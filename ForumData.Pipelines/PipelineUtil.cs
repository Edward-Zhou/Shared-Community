using LemonCore.Core;


namespace ForumData.Pipelines
{
    internal class PipelineUtil
    {
        public static bool BuildAndRun(Pipeline pipeline)
        {
            var exe = pipeline.Build();
            var task = exe.RunAsync(null);
            task.Wait();
            return task.Result;
        }
    }

}
