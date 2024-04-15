# KindleLister

Some work needs doing outside of the app to get started.

You need to have Kindle for Windows installed. (Yes, sorry, I've not tried this on Mac.)

Hit sync in your windows Kindle app.

Now if you go to C:\Users\<user>\AppData\Local\Amazon\Kindle\Cache\KindleSyncMetadataCache.xml
you will find your kindle list as an xml file

Go to https://codebeautify.org/xmltojson and paste the XML there

Now save out the xml as kindle_list.json in the root of your project

The path to the kindle_list.json is hard-coded so you'll need to fix it to match yours

The csv file will be output in the same dir as the json file