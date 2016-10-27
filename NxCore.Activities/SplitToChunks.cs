using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;

namespace NxCore.Activities
{
    // TODO Generic. Can be used to split joblists as well
    public sealed class SplitToChunks : CodeActivity<List<ImportTask>>
    {
        public InArgument<ImportTask> ImportTask { get; set; }
        public InArgument<int> FilesPerChunk { get; set; }

        protected override List<ImportTask> Execute(CodeActivityContext context)
        {
            ImportTask importTask = context.GetValue(ImportTask);
            int filesPerChunk = context.GetValue(FilesPerChunk);
           
            List<ImportTask> chunks = SplitList(importTask, filesPerChunk);

            return chunks;
        }

        static List<ImportTask> SplitList(ImportTask locations, int nSize = 30)
        {
            var chunks = new List<ImportTask>();

            for (int i = 0; i < locations.FilesToImport.Count; i += nSize)
            {
                ImportTask importTask = new ImportTask()
                {
                    FilesToImport = locations.FilesToImport.GetRange(i, Math.Min(nSize, locations.FilesToImport.Count - i)),
                    ImportSetName = locations.ImportSetName
                };
                chunks.Add(importTask);
            }          

            return chunks;
        }
    }

   
}
