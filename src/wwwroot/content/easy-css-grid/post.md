# Easy CSS grid

I've been working with a lot of forms lately and it required some kind of grid system to make it all lay out nicely. I've always been a bit hesitant to immediatly pull in another dependency and decided I was going to solve this one myself. How hard could it be?

By using the CSS flex layout, I ended up with just the right amount of flexibility and extensibility while keeping things very small code wise. Let's have a look!

Every line on the form is treated as a row in the grid, easy. These rows are divided up into columns, which can have variable widths and have to be elastic.

## Rows

This element will contain our columns and is very straight forward.

    .row {
        display: flex;
    }

That's all we need for rows.

## Columns

These are a tiny bit more involved, but not much. Since the element that contains these uses `flex`, we can flex these elements as well!

    .row .col {
        flex: 1;
    }

This requires a bit of explaining. The `1` in `flex: 1;` is called the flex factor. It tells the box model how much space an element will receive within its parent. Setting the flex factor on an element to `2` will cause that element to take twice as much space as an element that has a flex factor of `1`.

It's the same as setting `flex: 1 1 0;` which sets the grow and shrink factors to `1` and the basis to `0`. If you want to read more on what those values mean and do, [here's the official spec](https://www.w3.org/TR/css-flexbox-1/#flex-common). It's quite expansive but also a bit beyond the scope of this post :-)

## The grid

So at this point we have the most basic setup we can get, we can create rows and slice them up into evenly spaced columns.

![Simple columns](/content/easy-css-grid/simple-columns.png)

This gets us quite a long way already, but I also need columns that are constrained in some way. For this example, I'll add columns that always take 1/4th of the available space.

    .row .col-1-4 {
        flex: 1 1 25%;
        max-width: 25%;
    }

Time for a little bit of an explanation. Instead of just `flex: 1;` we are now setting all the flex properties in one go. We still allow growing and shrinking but we also define a basis (in this case 25%) from which the element will start resizing. To make sure the element never goes beyond the width we want it to be, we set a `max-width` to keep it from growing past a certain point (also 25% in the example).

With this column definition we can create columns that are still flexible but never grow beyond a certain size.

![Fixed width columns](/content/easy-css-grid/fixed-width-columns.png)

With this example in hand, we can now create all sorts of layouts any way we like.

![Mixing it up](/content/easy-css-grid/mixed-columns.png)

Creating new sizes is very easy, just add more rules for any size you want.

    .row .col-1-2 {
        flex: 1 1 50%;
        max-width: 50%;
    }

The CSS above will create a column that will take up half the space of a row. Any width, even fixed width is possible, just replace the percentage with a value in `px`.

The flex layout can help you build amazing things, even when it all seems so simple.

## Full source for this post

I could have put everything into a github repo, but this is short enough to include right here:

### CSS

    .row {
        display: flex;
    }

    .row .col {
        flex: 1;
    }

    .row .col-1-4 {
        flex: 1 1 25%;
        max-width: 25%;
    }

### Demo HTML

    <div class="row">
        <div class="col col-1-4">Col 1/4</div>
        <div class="col">Col</div>
    </div>

    <div class="row">
        <div class="col">Col</div>
        <div class="col col-1-4">Col 1/4</div>
    </div>

    <div class="row">
        <div class="col col-1-4">Col 1/4</div>
        <div class="col">Col</div>
        <div class="col col-1-4">Col 1/4</div>
    </div>
