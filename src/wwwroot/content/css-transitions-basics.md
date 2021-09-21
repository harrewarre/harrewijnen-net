{
    "title": "CSS Transitions basics",
    "summary": "End-users interact with your applications via the user interface (UI for short). Here are some simple things to spice it up.",
    "slug": "css-transitions-basics",
    "tags": [
        "CSS",
        "Transitions",
        "itq"
    ],
    "created": "2015-05-19"
}
---
# CSS Transitions basics

End-users interact with your applications via the user interface (UI for short). If everything was designed with the user in mind, the UI will make things very easy for the user. Everything is nice and organized and easy to navigate. Actions are communicated to the user in a friendly way and the user is happy.

The sad thing is, state changes in the UI tend to be very abrupt and cold. Instant flashes, so fast the user might not even notice if it is something small. So what can we do about this? CSS supports something called transitions. Transitions allow us to specify what happens what state changes on an element. For example, a button going from normal, to being hovered on by the user, rows in a grid that the user can select or a signal telling the user something happened.

So we know CSS can so these things but as always, [better safe than sorry](http://caniuse.com/#search=transitions). Those old browsers are still out there. Lucky for us, transitions degrade very nice since there is no change to the styles that were already used. We're only adding some sugar on top. Lets look at some examples.

<p data-height="268" data-theme-id="14183" data-slug-hash="LVZvve" data-default-tab="result" data-user="Harrewarre" class='codepen'>See the Pen <a href='http://codepen.io/Harrewarre/pen/LVZvve/'>Some simple transitions</a> by Sander Harrewijnen (<a href='http://codepen.io/Harrewarre'>@Harrewarre</a>) on <a href='http://codepen.io'>CodePen</a>.</p>
<script async src="//assets.codepen.io/assets/embed/ei.js"></script>

The button example is very straight forward, so lets have a look at what is going on. The first button has no transitions defined, the change in background is instant (and not very subtle). The other button does have transitions defined. The background color will transition when the **:hover** state changes. So when the user hovers over the second button, the browser will create a smooth animation between the normal and hovered states that lasts for as long as the transition dictates (half a second in this case). **Ease** tells the browser to slowly start and end the transition, varying the speed of the transition. Various options are available such as ease-in and ease-out.

The second example works in a similar way, except the transition acts on the **:checked** state to highlight the label that belongs to the checkbox. As you can see in the example, you can define more than one transition by separating them by a **,** (comma). If you want to transition every property of a class you can use the **transition: all (duration) (easing)** shortcut.

Before you start adding transitions to ALL the things, be aware that effects such as these can be very intrusive and distracting. Don't overdo it.

To wrap things up, transitions are defined on the beginning and ending state (such as :checked and normal states), a duration specifies how long the transition runs for and a timing function (easing) allows you to control how fast and/or slow the animation runs in the specified time. Remember, don't overdo it. Turning your UI into a glowing bouncing disco floor might not be the best user experience.