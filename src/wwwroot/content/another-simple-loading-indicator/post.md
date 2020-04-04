# Another simple loading indicator

Here is another loading indicator for when your webpage is busy waiting for a server request to come back with an answer. It's a bar at the very top of the page this time.

Let's look at a demo before we dive in:

<p data-height="268" data-theme-id="14183" data-slug-hash="vLJVbo" data-default-tab="result" data-user="Harrewarre" class='codepen'>See the Pen <a href='http://codepen.io/Harrewarre/pen/vLJVbo/'>Another simple loading indicator</a> by Sander Harrewijnen (<a href='http://codepen.io/Harrewarre'>@Harrewarre</a>) on <a href='http://codepen.io'>CodePen</a>.</p>
<script async src="//assets.codepen.io/assets/embed/ei.js"></script>

Press the button to do some imaginary tasks that simulates a workload. A rolling bar will appear at the top of the demo to show that something is happening in the background. So how does it work?

Let's get the Javascript out of the way first. All it does is add/remove CSS classes to and from the `busyBar` element. It simulates waiting for something using a `setTimeout`.

The HTML is very simple as well, 2 divs and a button. The first `div` represents the loading indicator, named `busyBar` and the second is a little container where our button lives. When you click the button, a Javascript functin is called that does all the work.

Now for the CSS part. The indicator is locked to the top of the window with `position: fixed` and `top, right and left` set to 0. That way, the bar will be visible even when the page is scrolled down but still be out of the way against the top of the document. We add some eye candy to it in the form of a background image and some shading using `box-shadow`.

Next we have the `active` and `hidden` classes for the indicator. These describe the states of the indicator (either in view, or not). We control it's visibility using the `height` of the div, which also allows us to add a nice little transition to it when it shows or hides itself.

To make the background move, or in this case, create the illusion of a spinning candy cane type thing, we use an `animation`. To describe the animation we use `@keyframes` with the name `rollaround` since that's what we used in the styles for the indicator itself. The `to` and `from` states move the background from top to bottom (0 to 100%) by adjusting the `background-position`. This is what creates the rotation illusion. To control the speed we can set the time 1 cycle of the animation takes, in this case 15 seconds for one whole cycle. Because we start the animation immediately we don't use any easing functions and just set the animation to run at a `linear` page. We also never want to stop animating so the duration is set to `infinite`.

To make use of the indicator you can set the `active` and `hidden` classes in functions/callbacks that you know might take a little longer. By showing the indicator, your users will get a visual hint that something is happening and they need to wait just a bit longer.