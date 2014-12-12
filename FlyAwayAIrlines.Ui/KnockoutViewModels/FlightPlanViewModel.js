var flightPlanViewModel;


function FlightPlan(flightPlanId, aircraftName, originAirport, destinationAirport, departureTime, arrivalTime, seatPrice) {
    var self = this;

    self.FlightPlanId = ko.observable(flightPlanId);
    self.AircraftName = ko.observable(aircraftName);
    self.OriginAirport = ko.observable(originAirport);
    self.DestinationAirport = ko.observable(destinationAirport);
    self.DepartureTime = ko.observable(departureTime);
    self.ArrivalTime = ko.observable(arrivalTime);
    self.SeatPrice = ko.observable(seatPrice);
}

function FlightPlanList() {
    var self = this;

    self.flightPlans = ko.observableArray([]);
    //self.flightPlans.removeAll();

    self.flightPlans.push(new FlightPlan(1, "Wings 1", "Ohare Airport", "Bradley  Airport", "12/15/2014 8:00 AM", "12/15/2014 12:00 PM", "230.00"));
    self.flightPlans.push(new FlightPlan(2, "Wings 2", "Bradley Airport", "Logan Airport", "12/15/2014 8:00 AM", "12/15/2014 12:00 PM", "450.00"));
    self.flightPlans.push(new FlightPlan(3, "Wings 3", "OHare Airport", "Logan Airport", "12/15/2014 8:00 AM", "12/15/2014 12:00 PM", "66.00"));
    self.flightPlans.push(new FlightPlan(4, "Wings 4", "Newark Airport", "San Francisco Airport", "12/15/2014 8:00 AM", "12/15/2014 12:00 PM", "770.00"));
    self.flightPlans.push(new FlightPlan(5, "Wings 5", "Logan Airport", "Bradley Airport", "12/15/2014 8:00 AM", "12/15/2014 12:00 PM", "88.00"));
    self.flightPlans.push(new FlightPlan(6, "Wings 6", "Logan Airport", "Bradley Airport", "12/16/2014 8:00 AM", "12/15/2014 12:00 PM", "400.00"));
    self.flightPlans.push(new FlightPlan(7, "Wings 7", "Bradley Airport", "Detroit Airport", "12/16/2014 8:00 AM", "12/15/2014 12:00 PM", "350.00"));
    self.flightPlans.push(new FlightPlan(8, "Wings 8", "Logan Airport", "Cincinatti Airport", "12/17/2014 8:00 AM", "12/15/2014 12:00 PM", "99.00"));
    self.flightPlans.push(new FlightPlan(9, "Wings 9", "Logan Airport", "Bradley Airport", "12/18/2014 8:00 AM", "12/15/2014 12:00 PM", "60.00"));
    


    console.info("Built Flight Plan List");
    console.info(self.flightPlans.count);

    // remove user. current data context object is passed to function automatically.
    self.bookFlight = function (flightPlan) {

        var dataObject = ko.toJSON(this);

        $.ajax({
            url: "/api/flightplans/",
            type: "post",
            data: dataObject,
            contentType: "application/json",
            success: function () {
                toastr.success("Your flight request has been submitted for processing. You will recieve your confirmation via email.");
            },
            error: function (request, status, error) {
                toastr.error(status.reason);
            }
        });
    };
}

// create index view view model which contain sub models for partial views
flightPlanViewModel = {
    availableFlightsViewModel: new FlightPlanList()
};


$(document).ready(
    function () {
        ko.applyBindings(flightPlanViewModel);
    });


ko.bindingHandlers.date = {
    update: function (element, valueAccessor, allBindingsAccessor) {
        var value = valueAccessor(), allBindings = allBindingsAccessor();
        var valueUnwrapped = ko.utils.unwrapObservable(value);

        var d = "";
        if (valueUnwrapped) {
            var m = /Date\([\d+-]+\)/gi.exec(valueUnwrapped);
            if (m) {
                d = String.format("{0:dd/MM/yyyy}", eval("new " + m[0]));
            }
        }
        $(element).text(d);
    }
};
ko.bindingHandlers.money = {
    update: function (element, valueAccessor, allBindingsAccessor) {
        var value = valueAccessor(), allBindings = allBindingsAccessor();
        var valueUnwrapped = ko.utils.unwrapObservable(value);

        var m = "";
        if (valueUnwrapped) {
            m = parseInt(valueUnwrapped);
            if (m) {
                m = String.format("{0:n0}", m);
            }
        }
        $(element).text(m);
    }
};