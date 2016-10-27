using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.ComponentModel;
using System.Windows.Markup;
using System.Activities.Tracking;

namespace NxCore.Activities
{
    [DesignerCategory("Profile Cutting")]
    public sealed class ImportProfileParts : SimulateWorkAsync
    {
        //public int TimePerNestingResult { get; set; }
        public InArgument<ImportTask> ImportTask { get; set; }

        protected override void TrackWorkResults(AsyncCodeActivityContext context)
        {
            // Track chunk ready
            var importTask = context.GetValue(ImportTask);
            string message = importTask.FilesToImport[0] + "-" + importTask.FilesToImport[importTask.FilesToImport.Count - 1];

            CustomTrackingRecord resultRecord = new CustomTrackingRecord("result", System.Diagnostics.TraceLevel.Info);
            resultRecord.Data["ImportTask"] = "ImportTask"  + message;
            context.Track(resultRecord);
        }

        //protected override int DoWork(int max)
        //{            
        //    Console.WriteLine("Start working on " + WorkDescription);

        //    for (int i = 0; i < max; i++)
        //    {


        //        System.Threading.Thread.Sleep(TimePerNestingResult);
        //    }            

        //    Console.WriteLine("Done working on " + WorkDescription);

        //    return 1;
        //}
    }

    ///// <summary>
    ///// Activity based on NativeActivity<TResult>
    ///// </summary>
    //[ContentProperty("Body")]    
    //[DesignerCategory("Profile Cutting")]
    //[Designer(typeof(Design.ImportProfilePartsDesigner))]
    //public sealed class ImportProfileParts : NativeActivity
    //{
    //    [RequiredArgument]
    //    [DefaultValue(null)]
    //    public InArgument<ImportTask> ImportTask { get; set; }

    //    //[RequiredArgument]
    //    //[DefaultValue(null)]
    //    //public InArgument<Database> Database { get; set; }

    //    [DefaultValue(null)]
    //    [Browsable(false)]
    //    public Activity Body { get; set; }

    //    protected override void Execute(NativeActivityContext context)
    //    {


    //    }

    //    /// <summary>
    //    /// Register activity's metadata
    //    /// </summary>
    //    /// <param name="metadata"></param>
    //    //protected override void CacheMetadata(NativeActivityMetadata metadata)
    //    //{
    //    //    // Register In arguments
    //    //    RuntimeArgument filesArg = new RuntimeArgument("FilesToImport", typeof(List<String>), ArgumentDirection.In);
    //    //    metadata.AddArgument(filesArg);
    //    //    metadata.Bind(this.FilesToImport, filesArg);

    //    //    //metadata.AddChild(new System.Activities.Statements.WriteLine());

    //    //    // [Text] Argument must be set
    //    //    if (this.FilesToImport == null)
    //    //    {
    //    //        metadata.AddValidationError(
    //    //            new System.Activities.Validation.ValidationError(
    //    //                "[FilesToImport] argument must be set!",
    //    //                false,
    //    //                "FilesToImport"));
    //    //    }

    //    //    RuntimeArgument importSetArg = new RuntimeArgument("ImportSetName", typeof(List<String>), ArgumentDirection.In);
    //    //    metadata.AddArgument(importSetArg);
    //    //    metadata.Bind(this.ImportSetName, importSetArg);

    //    //    // [Text] Argument must be set
    //    //    if (this.ImportSetName == null)
    //    //    {
    //    //        metadata.AddValidationError(
    //    //            new System.Activities.Validation.ValidationError(
    //    //                "[ImportSetName] argument must be set!",
    //    //                false,
    //    //                "ImportSetName"));
    //    //    }

    //    //    // [Body] Argument must be set 
    //    //    if (this.Body == null)
    //    //    {
    //    //        metadata.AddValidationError(
    //    //        new System.Activities.Validation.ValidationError(
    //    //        "[Body] argument must be set!",
    //    //        false,
    //    //        "Body"));
    //    //    }
    //    //    else
    //    //    {
    //    //        metadata.AddChild(this.Body);
    //    //    }

    //    //    // TODO : Add arguments ... etc ...
    //    //}

}
