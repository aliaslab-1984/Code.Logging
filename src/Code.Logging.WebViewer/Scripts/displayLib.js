window.displayLib = (function () {
    var md = {};

    function logEvent(data) {
        var self = this;

        self._data = data;

        var formatters = {
            TimeStamp: function (p) {
                var d = new Date(p);
                return { expand: false, value: d.getFullYear()+"/"+(d.getMonth()+1)+"/"+d.getDate()+" "+d.getHours()+":"+d.getMinutes()+":"+d.getSeconds()+"."+d.getMilliseconds(), obj: p };
            },
            Level: function (p) { return { expand: false, value: p.m_levelName, obj: p }; },
            Properties: function (p) { return { expand: true, value: "Properties", obj: p } },
            LocationInfo: function (p) { return { expand: true, value: "LocationInfo", obj: p } }
        };

        self.get = function (name) {
            return formatters[name] ? formatters[name](self._data[name]) : { expand: false, value: self._data[name] };
        };

        self.getRaw = function (name) {
            return self._data[name];
        };

        self.toString = function () {
            return JSON.stringify(self._data);
        }
        
        self.columns = function () {
            var ord = ["TimeStamp", "LoggerName", "Message", "Properties"];
            var all = Object.keys(self._data);
            all.forEach(function (p) {
                if (ord.indexOf(p) < 0)
                    ord.push(p);
            });

            return ord;
        }
    }

    md.convertAll = function (obj) {
        var res = [];
        var tmp = null;
        while ((tmp = obj.shift())) {
            res.push(new logEvent(tmp));
        }

        return res;
    };

    md.getColums = function (objs) {
        if (objs instanceof Array && objs.length == 0)
            return [];
        else if (objs instanceof Array == false)
            return objs.columns() || [];
        else
            return objs[0].columns() || [];
    };

    return md;
})();