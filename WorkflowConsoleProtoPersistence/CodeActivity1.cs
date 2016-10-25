using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;

namespace WorkflowConsoleProtoPersistence
{

    public sealed class CodeActivity1 : CodeActivity
    {
        public OutArgument<Guid> WFInstanceId { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            context.SetValue(WFInstanceId, context.WorkflowInstanceId);

        }
    }
}
