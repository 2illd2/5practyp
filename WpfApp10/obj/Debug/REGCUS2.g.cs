﻿#pragma checksum "..\..\REGCUS2.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "1F23D1C08F407BEE5D4177FBFCE511C0A5E2993B15E36A304D651EF1CDB1A7BC"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using WpfApp10;


namespace WpfApp10 {
    
    
    /// <summary>
    /// REGCUS2
    /// </summary>
    public partial class REGCUS2 : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 41 "..\..\REGCUS2.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox FirstNameTextBoxR;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\REGCUS2.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox LastNameTextBoxR;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\REGCUS2.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox MiddleNameTextBoxR;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\REGCUS2.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox EmailTextBoxR;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\REGCUS2.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox PhoneTextBoxR;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\REGCUS2.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox UsernameTextBoxR;
        
        #line default
        #line hidden
        
        
        #line 57 "..\..\REGCUS2.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox PasswordTextBoxR;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/WpfApp10;component/regcus2.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\REGCUS2.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.FirstNameTextBoxR = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.LastNameTextBoxR = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.MiddleNameTextBoxR = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.EmailTextBoxR = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.PhoneTextBoxR = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.UsernameTextBoxR = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.PasswordTextBoxR = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 8:
            
            #line 59 "..\..\REGCUS2.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.RegButton_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 61 "..\..\REGCUS2.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Back_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

