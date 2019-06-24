// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace XamarinNL
{
    [Register ("LanguageTokenizerViewController")]
    partial class LanguageTokenizerViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton FindWordsButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField UserInput { get; set; }

        [Action ("FindWordsButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void FindWordsButton_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (FindWordsButton != null) {
                FindWordsButton.Dispose ();
                FindWordsButton = null;
            }

            if (UserInput != null) {
                UserInput.Dispose ();
                UserInput = null;
            }
        }
    }
}