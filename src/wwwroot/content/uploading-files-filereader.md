{
  "slug": "uploading-files-filereader",
  "title": "Uploading files - a simple approach",
  "summary": "Upload (smallish) images (with preview) with some Javascript and HTML",
  "tags": [
    "HTML",
    "Javascript",
    "itq"
  ],
  "created": "2017-07-31T00:00:00"
}
---
# Uploading files - a simple approach

Here's a nice way of getting images to your server using a little bit of Javascript and HTML. We'll be using the `FileReader` API that comes with 
Javascript and is supported in [all](http://caniuse.com/#search=filereader) major browsers.

For this example we'll need some HTML from which we can select files and show a preview:

    <div>
        <input id="fileInput" type="file" />
        <button id="clearFile">Clear</button>
        <button id="uploadFile">Upload</button>
    </div>
    <div>
        <label>Preview</label>
        <div>
            <img id="preview" />
        </div>
    </div>

We have an `input` element set to `type="file"` to select the file(s) we want to upload. Two `button`s for submitting and clearing our inputs and an `img` where we can show a preview.
This demo only depends on jQuery, which is only used to make performing a POST to the webserver so much simpler. Feel free to substitute that with whatever you like best.

The webserver that will receive the image is ASP.NET but it should work on any other platform/server you want to use.

I've created a file called `app.js` and referenced it just before the closing `body` tag on my page. It contains a self-executing piece of code that wires up our HTML.

	(function ($) {
		var self = this;

		// Code will go here.
	})($);

It takes in jQuery (`$`), but as mentioned, feel free to substitute that with something else, it is only used for a POST to the server.

Let's get some of the boilerplate stuff out of the way first.

    self.fileInput = document.getElementById("fileInput");
    self.upload = document.getElementById("uploadFile");
    self.clear = document.getElementById("clearFile");
    self.preview = document.getElementById("preview");
	
Grab the elements that we need for any functionality and wire them up.

    self.upload.addEventListener("click", function () {
        if (self.fileInput.length === 0) {
            alert("No file to preview!");
            return;
        }

        var selectedFile = self.fileInput.files[0];
        self.getFile(selectedFile, function (fileData) {
            var uplReq = $.post("/api/upload", { name: fileData.name, base64String: self.getBase64String(fileData.dataUrl) });
            uplReq.done(function () {
                alert("File uploaded");
            });
        });
    });

    self.fileInput.addEventListener("change", (event) => {

        if (self.fileInput.length === 0) {
            alert("No file to preview!");
            return;
        }

        var selectedFile = self.fileInput.files[0];

        self.getFile(selectedFile, function (fileData) {
            self.preview.src = fileData.dataUrl;
        });
    });

Wire up the Clear and Submit buttons.

Here's the first little piece of utility we need:

    self.getBase64String = function (dataUrl) {
        return dataUrl.substr(dataUrl.indexOf(",") + 1);
    };

This little function reads a data URL and returns only the data part of it. To get a better understanding of it, here's an example. Here is the data URL for a PNG image (I've snipped of really long and boring part of the URL). The browser can read these URLs and, in the case of images even display them in an `img` element.

	<img id="preview" src="data:image/png;base64,iVBORw0KGgogbPG ... snip ... 5pTJUORK5CYII=">

We only want to send the data part of the URL to the server so we find the `,` in the data URL that seperates the metadata and the actual data and are left with the long string representation of the image. In the code above, we also wired up the `change` event on the file input
to respond the a file being selected. When the user selects a file we want to show a preview, to do so, we need the a `FileReader` to grab the file contents and give them to us for further processing.

In the piece of code that wires up the `change` event on the file input, we call a function `getFile`, here, the meat of it:

    self.getFile = function (file, onFileLoaded) {
        var result = {
            name: file.name,
            dataUrl: ""
        };

        var reader = new FileReader();
        reader.onload = function (event) {
            var url = event.target.result;
            result.dataUrl = url;

            onFileLoaded(result);
        };

        reader.readAsDataURL(file);
    };

It's the piece of code we're after to grab a preview and the data we want to send to the server. So when the user selects a file, the `change` event will fire and when everthing checks out, we end up in the `getFile` function.
Frist we prepare the return value that will contain the name of the uploaded file and it's data URL. Because the `FileReader` is event driven, we have to wait for the `onload` event to fire and call back into our code.

After the `onload` is hooked up we can tell the `FileReader` to get to work by calling the `readAsDataURL` function on it, passing in the file from the fileinput. When the browser finishes loading the file, 
the `onload` will be triggered and we can check the contents of the result for our data. We set the resulting URL (which is what we asked for by calling `readAsDataURL`) on the return value we've prepared and pass that into the `onFileLoaded` callback function we passed in with the file.

In the `change` event on the file input, the `onFileLoaded` callback is fired which sets the `src` property of the preview image to the data URL we got from the `FileReader`. We do the same thing when the submit button is clicked.
We grab the file using the `getFile` function and pass the result to an HTTP request to the server.

So why is this usefull? Well, we get a preview for the image (this obviously works for images only) and the data is now available in the same form as any other data that needs to be submitted. You can create a single form
that submits an entire user profile in one go, picture and all. The only catch is that large files result in really large base64 strings which can be a bit unwieldy and could bog things down on both the client and server. You can do some early file extension validation to ensure users are only uploading files that you support in the `FileReader` `onload` event, make sure you also check if the file is valid on the
server where you process it, never trust user input.

For smallish images, this is a nice trick to let the user preview what he/she is about to submit together with some over data you want to gather with your form. 

Here's a working demo without the posting to a server part:

<p data-height="300" data-theme-id="14183" data-slug-hash="JyKePQ" data-default-tab="js,result" data-user="Harrewarre" data-embed-version="2" data-pen-title="A simple FileReader demo" class="codepen">See the Pen <a href="https://codepen.io/Harrewarre/pen/JyKePQ/">A simple FileReader demo</a> by Sander Harrewijnen (<a href="https://codepen.io/Harrewarre">@Harrewarre</a>) on <a href="https://codepen.io">CodePen</a>.</p>
<script async src="https://production-assets.codepen.io/assets/embed/ei.js"></script>