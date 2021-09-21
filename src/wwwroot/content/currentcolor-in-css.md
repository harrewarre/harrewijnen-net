{
    "title": "CurrentColor in CSS",
    "summary": "If you've ever used something like LESS or SASS you know what a variable is in CSS.",
    "slug": "currentcolor-in-css",
    "tags": [
        "CSS",
        "itq"
    ],
    "created": "2015-06-22"
}
---
# Currentcolor in CSS

If you've ever used something like LESS or SASS you know what a variable is in CSS. There is also a working draft for native [CSS variables](http://www.w3.org/TR/css-variables-1/) but there is something in CSS right now that resembles some variable behavior. Not an actual variable, but interesting nonetheless.

Meet [currentColor](http://www.w3.org/wiki/CSS3/Color/currentColor). Like the description says on the w3 wiki, its value is whatever the `color` is set to. So how does that work? Lets look at some examples.

<p data-height="268" data-theme-id="14183" data-slug-hash="gpXOMm" data-default-tab="result" data-user="Harrewarre" class='codepen'>See the Pen <a href='http://codepen.io/Harrewarre/pen/gpXOMm/'>currentColor samples</a> by Sander Harrewijnen (<a href='http://codepen.io/Harrewarre'>@Harrewarre</a>) on <a href='http://codepen.io'>CodePen</a>.</p>
<script async src="//assets.codepen.io/assets/embed/ei.js"></script>

Sample one shows `currentColor` in its simplest form. It takes on the color Red which controls the border color.

Sample two is a tiny bit more complex. Here you can see the `currentColor` cascading down (Blue in this case) which sets a blue box-shadow on the `inner-div`.

Sample three is where we see that `currentColor` isn't a variable like the variables we know from things like Javascript or C#. We start by setting the `color` to Green and using `currentColor` to set the border color. Then we set the `color` to Yellow to change the background color. You can see it go wrong immediately, the text and border isn't green! This is because you can't re-assign `currentColor`. It can only be set once for every style and takes the value of the last time the `color` was set so in the sample it turns everything Yellow. When `color` is reset, `currentColor` is also reset as you can see, but cannot be set multiple times for the same set of styles. The inner div has black text and box-shadow because this has a different scope.

So in short, kind of like a variable but not actually a variable. I don't see [currentColor](http://caniuse.com/#search=currentColor) used very often in projects I get to work on, though its pretty safe to use (unless you support IE8). It can be used to save yourself some problems in dealing with consistent colors but you might be better off using [LESS](http://lesscss.org/) or [SASS](http://sass-lang.com/) if you want to use variables :-)