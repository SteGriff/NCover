NCover
------

## Goals

-   Simplicity
-   Speed - it doesn't profile, it just checks coverage.
-   Designed to be dropped into a continuous build cycle
-   Keep line numbers the same -&gt; Test failure stacktraces stay
    useful

## Upgrading from previous NCover versions

There are three breaking changes with this new release, these are:
To compile instrumented code, you will now need to reference the
NCover.dll
The NAnt tasks have been seperated out into a seperate assembly
(NCoverNAnt.dll), so you will now have to reference this assembly in the
loadtasks part of the nant build file.
The ncoverreport task now takes a mandatory parameter named "actuals"
which is a filename pointing to the output from running the tests upon
the instrumented code.
Please see the [docs](nant-integration.html) for further details.

## News

### 2013-01-09

-   Version 0.9.2 released see [changelog](CHANGES.md) for details
-   Now supports Visual Basic.NET code!
-   We've enhanced the diff functionality so you can see how changes to
    the code have affected the coverage percentage (for better or
    for worse).
-   Thanks to ideas from Michael Luke, setting up ncover is now easier
    than before - you just have to link to the dll. (well maybe a little
    bit more than that is needed, but it's easier than it was.)
    (Thanks Michael)

Author: [gilescope@fastmail.fm](emailto:gilescope@fastmail.fm)  

We'd greatly appreciate hearing about your success using this product,
and any problems you've encountered (the tighter the feedback loop, the better the product).
