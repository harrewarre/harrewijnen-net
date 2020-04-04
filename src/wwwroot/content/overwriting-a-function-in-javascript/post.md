# Overwriting a function in Javascript

Take the following piece of code:

    function Thing() {
        this.Load = function () {
            // Do stuff some specific way.
        };
    };
    

When we want to creat a new Thing we do:

    var newThing = new Thing();
    

Easy enough. But what if we have a corner case where the Thing needs to load data in another specific way?

    var newThing = new Thing();
    
    newThing.Load = function () {
        // Other specific doingstuff code.
    };
    

At first this might seem to be the right way to do it. There is a catch though, this code will not change a thing to the Thing object! The constructor of the Thing object adds the original Load function **after** we added our new Load funtion on the Thing. The constructor is overwriting our new function that was supposed to overwrite the original function.

So now what? Well, to get around this we need to redefine the constructor. Here is the code:

    var OtherKindOfThing = function() {
        Thing.apply(this, arguments);
    
        this.Load = function () {
            // Other specific doingstuff code.
        };
    };
    
    var newThing = new OtherKindOfThing();
    

This is what is going on. First we create a new constructor function (the example has no arguments, but don't forget those when you apply this pattern!). Inside our new constructor we run the old constructor against our current context (this) with our existing arguments (you could replace the arguments object with actual arguments if you wanted to pass in other arguments or whatever).

Now that we are scoped to the constructor of our alternative Thing, we can safely overwrite the Load function. Create the Thing using the new constructor function and you're all set!