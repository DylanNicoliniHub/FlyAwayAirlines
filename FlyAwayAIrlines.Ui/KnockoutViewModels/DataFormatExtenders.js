ko.extenders.currency = function (target, scale) {
    var cleanInput = function (value) {
        if (value || value === 0) {
            value = value + "";
            return parseFloat(value.replace(/[^0-9.\-]/g, ''));
        } else {
            return null;
        }
    };

    var format = function (value) {
        if (typeof value != 'undefined' && value != null) {
            var toks = value.toFixed(scale).replace('-', '').split('.');
            var display = '$' + $.map(toks[0].split('').reverse(), function (elm, i) {
                return [(i % 3 == 0 && i > 0 ? ',' : ''), elm];
            }).reverse().join('') + '.' + toks[1];

            return value < 0 ? '(' + display + ')' : display;
        } else {
            return null;
        }


    };

    var raw = typeof target == "function" ? ko.dependentObservable(target) : ko.observable(target);

    //create a writeable computed observable to intercept writes to our observable
    var result = ko.computed({
        read: ko.dependentObservable({
            read: function () {
                return raw();
            },
            write: function (value) {
                raw(cleanInput(value));
            }
        }), //always return the original observables value

        write: function (newValue) {
            newValue = cleanInput(newValue);
            var current = target(),
                roundingMultiplier = Math.pow(10, scale),
                newValueAsNum = newValue == "" || newValue == null ? 0 : parseFloat(+newValue);
            var valueToWrite;
            valueToWrite = !!newValue || newValue === 0 ? Math.round(newValueAsNum * roundingMultiplier) / roundingMultiplier : null;

            //only write if it changed
            if (valueToWrite !== current) {
                target(valueToWrite);
            } else {
                //if the rounded value is the same, but a different value was written, force a notification for the current field
                if (newValue !== current) {
                    target.notifySubscribers(valueToWrite);
                }
            }
        }
    });
    result.formatted = ko.dependentObservable({
        read: function () {
            return format(raw());
        },
        write: function (value) {
            result(cleanInput(value));
        }
    }); //always return the original observables value
    //initialize with current value to make sure it is rounded appropriately
    result(target());

    //return the new computed observable
    return result;
};



//Not part of the money observable, just wiring the viewModel up to the bindings.
$(function () {
    var viewModel = {
        Cash: ko.observable(1234.56).extend({
            currency: 2
        }),
        Check: ko.observable(2000).extend({
            currency: 2
        }),
        showJSON: function () {
            // alert(viewModel.NewCash.formatted());
            alert(ko.toJSON(viewModel));
        }
    };

    viewModel.Total = ko.computed(function () {
        return viewModel.Cash() + viewModel.Check()
    }).extend({ currency: 2 });
    ko.applyBindings(viewModel);
});