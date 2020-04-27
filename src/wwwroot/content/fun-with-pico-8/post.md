# Some fun with Pico-8

I had a some tine left in the day (who doesn't, with all the quarantine and all...) so why not spend that time programming. A long while back I picked up the [Pico-8](https://www.lexaloffle.com/pico-8.php) 
fantasy console. A tiny game engine, IDE and sprite editor all rolled into one. You write your stuff in Lua and are encouraged to keep things simple due to its (artifical) 
limitations.

Not really having a particular plan or goal, I ended up with a little robed character that can walk around on the screen.

<style>
    #walkman {
        display: block;
        margin: 0 auto;
        width: 100%;
        height: 50vh;
    }
</style>
<iframe id="walkman" src="/content/fun-with-pico-8/walkman/walkman.html"></iframe>
<p></p>
<p>
The """game""" uses the built-in tile map and continually checks the edges of the character for overlaps with tiles that cannot be walked on.
It seems to not handle collisions very well when the character moves very fast, which is a challenge for later.
</p>

If you want to play around with this a bit, [here is the Pico-8 cart](https://github.com/harrewarre/pico/tree/master/walkman) and source.