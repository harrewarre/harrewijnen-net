{ 
    "title": "It all starts with smaller stories",
    "slug": "start-with-small-stories",
    "summary": "Improve your development process by breaking down work into smaller pieces.",
    "tags": [
        "agility",
        "developer experience",
        "devex",
        "developer practices"
    ],
    "created": "2022-10-18"
}

---

# It all starts with smaller stories 

I think most developers have experienced a taks sitting in progress for days or even weeks (or... months!) on end. This long running work eventually gets merged and released but it turns out it's the wrong thing and a large part needs rework. 

The solution to this struggle is two-fold. The first half is getting invested in your customers. Talk to them, establish a good relation with the people that actually use the product you are building. Find out what they really need. This practice is known as User Centered Design and while I find it very interesting, it's not the topic of this post. We're looking at the other half, breaking down work into smaller pieces and the result of doing so. 

## Smaller pieces of work 

So, doing smaller pieces of work at a time sounds pretty simple, but I've had some serious push-back when trying to do this with some of the teams I've worked with. The big one is always "we don't need to make things this dumb, we're smart people, we know what to do!". It might seem like that when a story only explains the existence of a single button and its behavior instead of "add shopping cart functionality". 

Let's take the shopping cart example a bit further. How could we break that down? There's adding an item, removing an item, changing the quantity of an item, calculation of price (sub)totals and probably a bunch more stuff involved. That's already a breakdown into more stories. But we can take it even further. If we use the adding of items as an example, it's going to need a button on each product. That button has the effect of adding something to the cart. Adding the same thing once more has a different outcome than the first addition to the cart. Adding something that is out of stock has yet another outcome. See where this is going? You can keep cutting up the work into smaller chunks. 

## But what about the estimates? 

Now this is an interesting subject. I think the general consensus is that estimates are evil. They're always somewhat off the mark or even worse, quickly set in stone by the person asking for them and made into deadlines. 

If you've broken down your work into tiny pieces, estimation becomes a lot easier. Instead of having this entire shopping cart as the scope of estimation, you can now estimate things on a much smaller scope. As a general rule each little story should fit into about a half day worth of time. If it can't be done in roughly 4 hours, it's probably bigger than it should be. 

## Don't cross the streams! 

While these little stories can be knocked of the board quickly due to their size, there is one pitfall to watch out for. Streams of work. With these little stories it's very easy to cross into a part of the system that is also actively worked on. This can lead to conflicts that need resolving down the line using up time you shouldn't be wasting. 

So in our shopping cart example, all those little stories belong together as a stream of work. Though you might be able to create multiple streams by letting some team members start in the back-end and work forward and others work from the front towards the back-end. It'll depend on the chosen architecture and technical implementation. Just be mindful of streams of work. 

The number of streams should be matched to your team size. Let's say a team with 6 developers, consisting of 3 pairs can work on 3 streams simultaneously. Pairs working in the same stream should be avoided or they should be in close contact to avoid the above mentioned merge conflicts or overwriting each other's work. 

## Agility 

Something these small stories also provide is agility. Technically you should be able to produce a working release for every finished story. This means that every completed story could be validated with your end-users. If something is not up to spec or needs adjusting, you only need to backtrack a tiny bit instead of running into a pile of rework because you didn't validate anything for 3 weeks. 

## In practice 

So we've made estimating work easier and more accurate. Flow of work is increased because we can release more often in smaller chunks. We've also made it possible to respond to changing requirements faster because we can validate more often. The thing we need to watch out for is crossing streams of work, which is easily address with clear communication and a little up front planning. 

If there is one thing you should try if you want to increase your dev teams agility, it is writing smaller stories. It does take some practice, and will require some change in the way you work. To get even more juice from smaller stories, combine it with pair-programming and trunk-based development but that's going to take some additional technical maturity and we've also kind of glossed over testing practices.