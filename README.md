VKMinus
=======

This is a simple try to parse the contents of the local paper http://www.vk.se/ to see how many of their articles are behind their paywall.

It's a quite rough implementation and might not be 100% accurate but it seems to do a pretty OK job.

To get it working properly, you need a mongodb-database running somewhere and a file called connection.xml in the Data-project. The layout of the file should be as follows.

    <root>
      <connectionstring>mongodb://username:password@host:port/databasename</connectionstring>
    </root>

It isn't available at Github because I'm lazy and honestly don't think that anyone else will be intressted in this project.

[![Bitdeli Badge](https://d2weczhvl823v0.cloudfront.net/karl-sjogren/vkminus/trend.png)](https://bitdeli.com/free "Bitdeli Badge")

