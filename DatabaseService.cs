using SQLite;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Timeline;

public class DatabaseService
{
    private readonly SQLiteAsyncConnection _database;

    public DatabaseService(string dbPath)
    {
        try 
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<TimeEvent>().Wait();
            _database.CreateTableAsync<TimeEventData>().Wait();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
        }
           

        
    }

    // TimeEvent 操作
    public Task<List<TimeEvent>> GetTimeEventsAsync()
    {
        return _database.Table<TimeEvent>()
            .OrderBy(timeEvent => timeEvent.TimeCreated)
            .ToListAsync();
    }

    public Task<List<TimeEvent>> GetTimeEventsAsync(int timeEventId)
    {
        return _database.Table<TimeEvent>().Where(data => data.Id == timeEventId).ToListAsync();
    }

    public Task<int> SaveTimeEventAsync(TimeEvent timeEvent)
    {
        return _database.InsertAsync(timeEvent);
    }

    public async Task<bool> UpdateTimeEventAsync(int id, string newDescription, string newDefaultValue, bool isPrefernce)
    {
        var timeEvent = await _database.FindAsync<TimeEvent>(id);
        if (timeEvent != null)
        {
            // 更新属性
            timeEvent.Description = newDescription;
            timeEvent.DefaultValue = newDefaultValue;
            timeEvent.IsPreference = isPrefernce;

            // 执行更新
            var result = await _database.UpdateAsync(timeEvent);
            return result > 0; // 返回是否更新成功
        }
        return false; // 如果未找到条目，返回 false
    }

    public Task<int> DeleteTimeEventByIdAsync(int eventId)
    {
        return _database.DeleteAsync<TimeEvent>(eventId); // 根据 EventID 删除
    }

    // TimeEventData 操作
    public Task<List<TimeEventData>> GetTimeEventDataAsync(int timeEventId)
    {
        return _database.Table<TimeEventData>().Where(data => data.TimeEventId == timeEventId).OrderBy(TimeEventData=>TimeEventData.TimeCreated).ToListAsync();
    }

    public Task<int> SaveTimeEventDataAsync(TimeEventData timeEventData)
    {
        return _database.InsertAsync(timeEventData);
    }

    public Task<int> DeleteTimeEventDataByIdAsync(int TimeEventDataID)
    {
        return _database.DeleteAsync<TimeEventData>(TimeEventDataID); // 根据 EventID 删除
    }

    public async Task ClearDatabaseAsync()
    {
        await _database.DeleteAllAsync<TimeEvent>(); // 清空 TimeEvent 表
        await _database.DeleteAllAsync<TimeEventData>(); // 清空 TimeEventData 表
    }

}