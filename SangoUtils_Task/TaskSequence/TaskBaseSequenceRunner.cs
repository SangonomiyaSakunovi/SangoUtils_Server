using System;

namespace SangoUtils_Task
{
    public abstract class TaskBaseSequenceRunner
    {
        public Action<string>? LogInfoFunc { get; set; }
        public Action<string>? LogWarnningFunc { get; set; }
        public Action<string>? LogErrorFunc { get; set; }
    }
}