# The bug report

When you work on software, you will at some point encounter bugs. You think everything is peachy and you covered all the bases. Here comes this one user that finds a way to break your stuff.

So a user found a bug. He/she (hopefully) reports the bug and you can fix the problem. While this is true most of the time, the sad reality is that most bug reports I encounter look something like this:

> When the admin is also a manager, nothing will happen when I click on the Save button.

<small>This might be an oversimplification, but serves as a good example :-)</small>

If you are reading this and think "Hey, that's how I report bugs!" please keep reading.

Let's dismantle what the report says.

 - The user is an admin.
 - The user is also a manager.
 - The user wants to save this work.

Well that is something, but there is no context anywhere. What was being saved? What was typed in? User is a manager, but what kind of manager? It might be trivial if the application is small, but in large enterprise type software it can mean anything.

The amount of time it takes to gather all the pieces of evidence to reproduce a bug can be huge and isn't used to actually fix the underlying problem. Communicating back and forth with the customer and user, trying to get more information about what was happening when the bug showed itself. Its not a direct waste of time though, sometimes a bug is hard to reproduce, but anything that saves both parties time is a good thing.

If you find a bug in a piece of software, you can save everyone a good amount of time by taking note of what you were doing when the software messed up. Saying "so-and-so doesn't work" will waste time for everyone involved.

So what would a simple bug report look like? You can still describe what was going on when the bug appeared but expand a little bit. Here are some things you can include to make it a little easier (and faster!) for the developer to start fixing things:

**Error messages.** This is the easiest one, if the app gave you some description of what went wrong, include it. Any error messages, no matter how cryptic or ugly. Take a screenshot if you can, anything that captures the broken state of the software.

**Where the bug popped up.** This can be a URL or a series of clicks (for example: Home -> Users -> Detail -> Edit), anything that describes the path you took to get work done.

**What actions were taken.** Did you click on certain things to load some other data first? Did you enter values into a form of some kind? Whatever you entered, include that as well.

**Log files.** End users don't have access to these most of the time, but if there is a way to include any logs of the software, please do.

**Contact information.** It doesn't have to be the user that reported the bug. Having a way to quickly get in touch with someone who can provide a little bit more detail or insight is enough.

**Version number.** This will tell the developer if the bug might already be fixed if it was reported on an older version of the software.

There will always be bugs that are extra tricky, or require a very unique set of conditions to trigger so these can be hard to reproduce or even describe. Try to capture as much as you can before sending a report to the developer.

Thank you so much if you do this,
Every software developer ever :-)
