`Core` - Library project containing code to be shared across platforms  
`AndroidUsingCore` - Xamarin.Android project  
`iPhoneUsingCore` - Xamarin.iOS iPhone project  

Notes
-----
* Made with Xamarin Studio, but I assume everything will work the same way in VS
* The Android and iPhone projects each reference the compiled file `Core/bin/debug/Core.dll`