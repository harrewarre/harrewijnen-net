# Animations and transforms

Every employee at [ITQ](http://www.itq.nl) has their own little mini-me in the form of a little character based on your appearance. Here is mine:

![MiniMe](/content/animations-and-transforms/minime-front.png)

So I have this image sitting in a folder on my Ondrive for whenever I might need it. Queue a boring evening with not much else to do, I figured, lets animate some life into the little guy using CSS. Here is the result.

<p data-height="550" data-theme-id="14183" data-slug-hash="NGQJZb" data-default-tab="result" data-user="Harrewarre" class='codepen'>See the Pen <a href='http://codepen.io/Harrewarre/pen/NGQJZb/'>Animated MiniMe</a> by Sander Harrewijnen (<a href='http://codepen.io/Harrewarre'>@Harrewarre</a>) on <a href='http://codepen.io'>CodePen</a>.</p>
<script async src="//assets.codepen.io/assets/embed/ei.js"></script>

Use the tabs on the embedded code pen to take a peek under the hood. The animations use transforms to manipulate the div that contains the image. By using percentages instead of just `from` and `to` for the animation states we can take a lot more control over the flow of the animation.