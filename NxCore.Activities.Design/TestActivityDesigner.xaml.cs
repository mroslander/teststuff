using System;
using System.Activities.Presentation.Metadata;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NxCore.Activities.Design
{
    // Interaction logic for TestActivityDesigner.xaml
    public partial class TestActivityDesigner
    {
        public TestActivityDesigner()
        {
            InitializeComponent();
        }

        public static void RegisterMetadata(AttributeTableBuilder builder)
        {
            builder.AddCustomAttributes(typeof(TestActivity), new DesignerAttribute(typeof(TestActivityDesigner)));
            builder.AddCustomAttributes(typeof(TestActivity), new DescriptionAttribute("Sample Activity"));
        }
    }
}
