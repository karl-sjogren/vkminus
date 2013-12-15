window.renderedCharts = [];

function renderChart(id, count) {
    if (!!window.renderedCharts[id]) {
        return;
    }

    var element = document.getElementById(id);
    var el = $(element);

    var values = el.data("values").split(", ").slice(0, count);
    var labels = el.data("labels").split(", ").slice(0, count);

    var highest = 0;
    var lowest = 0;
    
    for (var i = 0; i < values.length; i++) {
        var value = values[i];
        
        if (value > highest) {
            highest = value;
        }
        
        if (value < lowest || lowest === 0) {
            lowest = value;
        }
    }
    
    highest = Math.ceil(highest * 1.1);
    lowest = Math.floor(lowest * 0.8);
    
    var lineChartData = {
        labels: labels,
        datasets: [
            {
                fillColor: "rgba(220,220,220,0.5)",
                strokeColor: "rgba(220,220,220,1)",
                pointColor: "rgba(220,220,220,1)",
                pointStrokeColor: "#fff",
                data: values
            }
        ],
    };

    var options = {        
        scaleOverride : true,
	    scaleSteps : highest - lowest,
	    scaleStepWidth : 1,
	    scaleStartValue : lowest
    };
    
    new Chart(element.getContext("2d")).Line(lineChartData, options);
    window.renderedCharts[id] = true;
}

(function() {
    if (Modernizr.mq('only all and (min-width: 1200px)')) {
        $(".main").onepage_scroll({
            sectionContainer: "section",
            easing: "ease", // Easing options accepts the CSS3 easing animation such "ease", "linear", "ease-in", "ease-out", "ease-in-out", or even cubic bezier value such as "cubic-bezier(0.175, 0.885, 0.420, 1.310)"
            animationTime: 1000,
            pagination: true,
            updateURL: false,
            loop: false,
            afterMove: function(idx) {
                if (idx === 1) {
                    renderChart("dailyAverage", 10);
                } else if (idx === 3) {
                    renderChart("mainAverage", 30);
                }
            }
        });
    } else {
        renderChart("dailyAverage", 10);
        renderChart("mainAverage", 10);
    }
})();