# MAL.Net - A .net API for MAL

[![Build status](https://ci.appveyor.com/api/projects/status/ys3wp59j87wf3kna?svg=true)](https://ci.appveyor.com/project/DeadlyEmbrace/mal-net)

A rebuild and expansion of the umalapi unofficial MAL API into C#.
The idea is to rebuild the API as a WCF service that also supports JSON for backward compatibility as well as the ability to support clients
that cannot or doesn't want to make use of WCF.

The API will be built using the awesome Html Agility Pack (https://htmlagilitypack.codeplex.com/) to do most of the scraping.
