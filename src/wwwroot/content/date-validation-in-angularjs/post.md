# Date validation in AngularJS

Another angular post! This time we take a look at how to create a directive that ensures the user enters a valid date (and/or time).

Let's start with some boilerplate stuff first. I'm going to assume that you have something working Angular already, we will only cover the date validation directive in this post.
Here is some HTML.


    <div ng-app="validDateApp">
        <div ng-form="myForm" ng-controller="testController">
            <input ng-model="myDate" my-valid-date name="myDateInput" type="text" />
        </div>
    </div>


And it's Angular app to go with it:

    var app = angular.module("validDateApp");

	app.controller("testController", function ($scope) {
		$scope.myDate = null;
	});

	app.directive("myValidDate", function () {
		// Magic here.
	});

Let's take a quick look at the basic structure of a directive. It's an object that Angular binds to nodes that have a certain predefined properties on them. In our case, we want to use an attribute (`my-valid-date`). When we put that on an element, Angular will run our directive against it and we want our element to have a model property (`ng-model` on the element) bound to it as well, because we need access to the routines that Angular uses for validation.


	app.directive("myValidDate", function () {
		return {
			restrict: "A",
			require: "ngModel",
			link: function () {
				// Magic now here.
			}
		}
	});


The above code is the skeleton we will use to create our date validation attribute. We restrict our directive to only attributes. By requiring `ngModel` we enforce that this directive only works on elements that specify an `ng-model`. The link function is our gateway into hooking up the stuff to let us validate the user input on the element.

We will be using [MomentJS](http://momentjs.com/) for the date validation in this directive so make sure you have it referenced in your page.

So we want moment to validate the user input. By requiring that `ngModel` is present, we get access to the modelcontroller that belongs to the input. Which in turn gives us the `$validators` object that is bound to the input!

knowing this, let's give the `link` function a body.

	link: function (scope, elem, attr, ctrl) {
		ctrl.$validators.myValidDate = function(value) {
			if (value === undefined || value === null || value === "") {
				return true;
			}

			return moment(value, ["D-M-YYYY"], true).isValid();
		}
	}

The 4th argument on the link function contains a reference to the modelcontroller we need for validation. If you want to know more about the other arguments, you can read more [here](https://docs.angularjs.org/guide/directive)

To get our validation to work, we add function (`myValidDate`) to the validators object. The function accepts the input that was supplied by the user. First we check if there was any input at all. We don't require input for the date validation. Use the `required` attribute for that.

If we did receive input, we create a moment object with the supplied input and check if it matches the format we want. The moment object exposes an `isValid()` [function](http://momentjs.com/docs/#/parsing/is-valid/) to check if the object contains a valid date. We simply return the result of the `isValid` call as the result of our validation.

Now you can make any input enforce a valid date format by added the `my-valid-date` attribute. If you want to take this another step forward, you could add the format string to the attribute and have the option of validating in different formats per input, but I'll leave that as an exercise :-)

To round this off, here is a working demo of the above descriptions. Enjoy!
<iframe width="100%" height="600" src="https://embed.plnkr.co/PsDnVPClv6d9D7hthUp2/"></iframe>

Or go [here](http://plnkr.co/edit/PsDnVPClv6d9D7hthUp2) for the full plunker.