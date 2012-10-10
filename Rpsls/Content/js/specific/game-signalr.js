var Stats = function () {
};

Stats.prototype.Table = {
    rock: { win: 0, lose: 0, tie: 0 },
    scissors: { win: 0, lose: 0, tie: 0 },
    paper: { win: 0, lose: 0, tie: 0 },
    lizard: { win: 0, lose: 0, tie: 0 },
    spock: { win: 0, lose: 0, tie: 0 },
    total: { win: 0, lose: 0, tie: 0 }
}

var AddLogView = function (options) {
    this.el = options.el;
    this.formatedDateHelper = options.formatedDateHelper;
}

AddLogView.prototype.add = function (text) {
    var message = this.formatedDateHelper.getNowLogFormatedDate() + " " + text + "<br />" + this.el.html();
    this.el.html(message);
}

var FormatedDateHelper = function () {
}

FormatedDateHelper.prototype.getNowLogFormatedDate = function () {
    var d = new Date();
    var curr_date = d.getDate();
    var curr_month = d.getMonth() + 1; //Months are zero based
    var curr_year = d.getFullYear();
    return "[" + curr_year + "-" + curr_month + "-" + curr_date + " " + d.getHours() + ":" + d.getMinutes() + ":" + d.getSeconds() + "." + d.getMilliseconds() + " ]";
}

var StatsView = function (options) {
    this.stats = options.stats;

    this.elTotalWinCount = options.elTotalWinCount;
    this.elTotalLoseCount = options.elTotalLoseCount;
    this.elTotalTieCount = options.elTotalTieCount;
    this.elTotalWinRate = options.elTotalWinRate;
    this.elTotalLoseRate = options.elTotalLoseRate;
    this.elTotalTieRate = options.elTotalTieRate;
    this.elTotalTotalCount = options.elTotalTotalCount;

}

StatsView.prototype.updateTotalStatus = function () {
    var total = this.stats.Table["total"]["win"] + this.stats.Table["total"]["lose"] + this.stats.Table["total"]["tie"];

    this.elTotalWinCount.html(this.stats.Table["total"]["win"]);
    this.elTotalLoseCount.html(this.stats.Table["total"]["lose"]);
    this.elTotalTieCount.html(this.stats.Table["total"]["tie"]);

    var rate = (this.stats.Table["total"]["win"] / total) * 100;
    this.elTotalWinRate.html(rate.toFixed(2));

    rate = (this.stats.Table["total"]["lose"] / total) * 100;
    this.elTotalLoseRate.html(rate.toFixed(2));

    rate = (this.stats.Table["total"]["tie"] / total) * 100;
    this.elTotalTieRate.html(rate.toFixed(2));

    this.elTotalTotalCount.html(total);
}

StatsView.prototype.setStats = function (gesture, result) {
    this.stats.Table[gesture][result]++;
    this.stats.Table["total"][result]++;

    var spanId = gesture + "-" + result + "-count";
    $("#" + spanId).html(this.stats.Table[gesture][result]);

    var total = this.stats.Table[gesture]["win"] + this.stats.Table[gesture]["lose"] + this.stats.Table[gesture]["tie"];
    $("#" + gesture + "-total-count").html(total);

    var rate = (this.stats.Table[gesture]["win"] / total) * 100;
    spanId = gesture + "-win-rate";
    $("#" + spanId).html(rate.toFixed(2));

    rate = (this.stats.Table[gesture]["lose"] / total) * 100;
    spanId = gesture + "-lose-rate";
    $("#" + spanId).html(rate.toFixed(2));

    rate = (this.stats.Table[gesture]["tie"] / total) * 100;
    spanId = gesture + "-tie-rate";
    $("#" + spanId).html(rate.toFixed(2));

    this.updateTotalStats();
}

var PageViewEvents = function (options) {
    var that = this;

    $("#dismiss-move").live('click', function () {
        $.unblockUI();
        rpslsHubClient.dismissMove();
    });

    $("a[id^='select-']").live('click', function () {
        var gestureType = $(this).attr("id").replace("select-", "");
        rpslsHubClient.sendMoveServer(gestureType);
        return false;
    });
}

var LegendView = function (options) {
    this.el = options.el
}

LegendView.prototype.write = function (legend) {
    this.el.html(legend);
}

var PlayerView = function (options) {
    this.el = options.el;
}

PlayerView.prototype.changeImageSrc = function (img) {
    this.el.attr("src", img);
}

var HubWrapper = function (options) {
    this.username = options.username;
    this.email = options.email;

    this.resultView = options.resultView;
    this.playerOneView = options.playerOneView;
    this.playerTwoView = options.playerTwoView;
    this.statsView = options.statsView;
    this.addViewLog = options.addViewLog;
    this.tpview = options.tpview;

}

HubWrapper.prototype.init = function () {
    // Start the connection
    var that = this;
    rpslsHubClient = $.connection.rpslsHub;

    $.connection.hub.start(function () {
        rpslsHubClient.join(that.username, that.email);
    });

    // Receive a new message from the server
    //result win, lose, tie
    rpslsHubClient.play = function (legend, myGesture, otherPlayergesture, result) {
        that.resultView.write(legend);
        that.playerTwoView.changeImageSrc("/Content/Images/" + otherPlayergesture + ".png");
        that.statsView.setStats(myGesture.toLowerCase(), result.toLowerCase());
        that.addViewLog.add(legend);

    };

    rpslsHubClient.moveAccepted = function (gesture) {
        that.playerOneView.changeImageSrc("/Content/Images/" + gesture + ".png");
        that.playerTwoView.changeImageSrc("/Content/Images/none.png");
    }

    rpslsHubClient.moveAcceptedToPlay = function (gesture) {
        that.playerOneView.changeImageSrc("/Content/Images/" + gesture + ".png");
    }

    // Receive a new message from the server
    rpslsHubClient.addMessage = function (message) {
        that.addViewLog.add(message);
    };

    rpslsHubClient.addWarning = function (messagetxt) {
        that.addViewLog.add(messagetxt);

        //argh!!!
        $('#question').html(messagetxt + "<a id='dismiss-move'>dismiss move.</a>")
        $.blockUI({ message: $('#question'), css: { width: '275px' } });
    };

    rpslsHubClient.totalPlayers = function (totalPlayers) {
        that.tpview.write(totalPlayers);
    };
}

var Game = function () {
}

Game.prototype.start =  function (options) {
        var stats = new Stats();
        var formatedDateHelper = new FormatedDateHelper();
        var addLogView = new AddLogView({
            el: options.addLogElement,
            formatedDateHelper: formatedDateHelper
        });

        var statsView = new StatsView({
            stats: stats,
            elTotalWinCount: options.elTotalWinCount,
            elTotalLoseCount: options.elTotalLoseCount,
            elTotalTieCount: options.elTotalTieCount,
            elTotalWinRate: options.elTotalWinRate,
            elTotalLoseRate: options.elTotalLoseRate,
            elTotalTieRate: options.elTotalTieRate,
            elTotalTotalCount: options.elTotalTotalCount
        });

        var resultView = new LegendView({
            el: options.resultViewElement
        });

        var totalPlayersView = new LegendView({
            el: options.totalPlayersView
        });

        var playerOneView = new PlayerView({
            el: options.playerOneView
        });

        var playerTwoView = new PlayerView({
            el: options.playerTwoView
        });

        var hubWrapper = new HubWrapper({
            username: options.username,
            email: options.email,
            resultView: resultView,
            playerOneView: playerOneView,
            playerTwoView: playerTwoView,
            statsView: statsView,
            addViewLog: addLogView,
            tpview: totalPlayersView
        });

        hubWrapper.init();

        new PageViewEvents({
        });
}