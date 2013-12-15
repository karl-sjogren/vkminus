
/*
 * GET home page.
 */

exports.index = function(req, res){
    if (process.argv[2] === "vsdebug") {
        // Go for test-data instead since VS won't load mongodb for some reason
        return res.render('index', {
            title: 'VK-',
            links: 161,
            plusLinks: 161,
            percentage: 34,
            latestMainLinks: 47,
            latestMainPlusLinks: 27,
            latestMainPercentage: 57,
            latestNewsLinks: 98,
            latestNewsPlusLinks: 21,
            latestNewsPercentage: 21,
            lastUpdated: '2013-12-12 14:02:00',
            block0: {
                links: 6,
                pluslinks: 2,
                percentage: 33
            },
            block1: {
                links: 7,
                pluslinks: 5,
                percentage: 71
            },
            block2: {
                links: 10,
                pluslinks: 7,
                percentage: 70
            },
            block3: {
                links: 10,
                pluslinks: 7,
                percentage: 70
            },
            block4: {
                links: 7,
                pluslinks: 4,
                percentage: 57
            },
            block5: {
                links: 8,
                pluslinks: 6,
                percentage: 75
            }
        });
    }
    
    var mongoClient = require('mongodb').MongoClient;
    var moment = require('moment');

    var percentage = function(val, total) {
        var res = parseFloat(val / total).toFixed(2);
        return Math.floor(res * 100);
    };

    mongoClient.connect(process.env.mongodb, function(err, db) {
        if (err) throw err;
        console.log("Connected to Database");
        
        var collection = db.collection('statistics');
        var cMain = collection.find();
        cMain.sort({ 'createdAt': 1 }).limit(1);
        
        cMain.nextObject(function(err, results) {
            if (err) throw err;
            
            var model = {
                title: 'VK-',
                links: results.TotalLinks,
                plusLinks: results.TotalPlusLinks,
                percentage: percentage(results.TotalPlusLinks, results.TotalLinks),
                latestMainLinks: results.TotalMainLinks,
                latestMainPlusLinks: results.TotalMainPlusLinks,
                latestMainPercentage: percentage(results.TotalMainPlusLinks, results.TotalMainLinks),
                latestNewsLinks: results.TotalLatestNewsLinks,
                latestNewsPlusLinks: results.TotalLatestNewsPlusLinks,
                latestNewsPercentage: percentage(results.TotalLatestNewsPlusLinks, results.TotalLatestNewsLinks),
                lastUpdated: moment(results.CreatedAt).format('YYYY-MM-DD HH:mm:ss'),
                block0: {
                    links: results.Blocks[0].TotalLinks,
                    pluslinks: results.Blocks[0].TotalPlusLinks,
                    percentage: percentage(results.Blocks[0].TotalPlusLinks, results.Blocks[0].TotalLinks)
                },
                block1: {
                    links: results.Blocks[1].TotalLinks,
                    pluslinks: results.Blocks[1].TotalPlusLinks,
                    percentage: percentage(results.Blocks[1].TotalPlusLinks, results.Blocks[1].TotalLinks)
                },
                block2: {
                    links: results.Blocks[2].TotalLinks,
                    pluslinks: results.Blocks[2].TotalPlusLinks,
                    percentage: percentage(results.Blocks[2].TotalPlusLinks, results.Blocks[2].TotalLinks)
                },
                block3: {
                    links: results.Blocks[3].TotalLinks,
                    pluslinks: results.Blocks[3].TotalPlusLinks,
                    percentage: percentage(results.Blocks[3].TotalPlusLinks, results.Blocks[3].TotalLinks)
                },
                block4: {
                    links: results.Blocks[4].TotalLinks,
                    pluslinks: results.Blocks[4].TotalPlusLinks,
                    percentage: percentage(results.Blocks[4].TotalPlusLinks, results.Blocks[4].TotalLinks)
                },
                block5: {
                    links: results.Blocks[5].TotalLinks,
                    pluslinks: results.Blocks[5].TotalPlusLinks,
                    percentage: percentage(results.Blocks[5].TotalPlusLinks, results.Blocks[5].TotalLinks)
                }
            };


            var cMainAverage = collection.find();
            cMainAverage.sort({ 'createdAt': 1 }).limit(30);

            cMainAverage.toArray(function(err, results) {
                if (err) throw err;

                var values = [];
                var labels = [];
                for (var i = 0; i < results.length; i++) {
                    var item = results[i];
                    values.push(percentage(item.TotalMainPlusLinks, item.TotalMainLinks));
                    labels.push(moment(item.CreatedAt).format('HHmm'));
                }

                values.reverse();
                labels.reverse();

                model.mainChart = { 
                    labels: labels.join(', '),
                    values: values.join(', ')
                };

                values = [];
                labels = [];
                for (var i = 0; i < results.length; i++) {
                    var item = results[i];
                    values.push(percentage(item.TotalPlusLinks, item.TotalLinks));
                    labels.push(moment(item.CreatedAt).format('HHmm'));
                }

                values.reverse();
                labels.reverse();

                model.totalChart = { 
                    labels: labels.join(', '),
                    values: values.join(', ')
                };

                console.dir(model);

                res.render('index', model);
                db.close();
            });
        });
    });
};