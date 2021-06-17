using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Covid19
{
    public class TaskHelper
    {
        public static async Task<T[]> WhenAll<T>(IEnumerable<Task<T>> tasks)
        {
            var taskResult = Task.WhenAll(tasks);
            try
            {
                var result = await taskResult;
                return result;
            }
            catch (Exception)
            {
            }

            throw taskResult.Exception ?? throw new Exception("Something Wrong Happenned");
        }
    }
}
