# Rate Schedule Sample Mobile API Stack

This sample project is a simple .NET Standard/Core Web API and Swift iOS app demonstrating a complete backend and frontend that allows a user to enter a date time range, and receive a rate at which they would be charged for that time span.

# Backend

## Setting Up a Rate Schedule

Before asking for rates, a schedule must be set up. This is accomplished using the `postschedule` endpoint.

### api/rates/postschedule

The `postschedule` endpoint accepts a HTTP POST call with rate data in the following format:

```
{
  "rates": [
    {
      "days": "mon,tues,thurs",
      "times": "0900-2100",
      "price": 1500
    },
    {
      "days": "fri,sat,sun",
      "times": "0900-2100",
      "price": 2000
    }
  ]
}
```

Curl example:
```
curl -H "Content-Type: application/json" -d '{"rates": [{"days": "mon,tues,thurs","times": "0900-2100","price": 1500},{"days": "fri,sat,sun","times": "0900-2100","price": 2000},{"days": "wed","times": "0600-1800","price": 1750},{"days": "mon,wed,sat","times": "0100-0500","price": 1000},{"days": "sun,tues","times": "0100-0700","price": 925}]}' http://localhost:5000/api/rates/postschedule
```

The current implementation does nothing to prevent duplicate or overlapping times.

## Getting All Rates

A GET call to the `rates` endpoint will return all rates.

### api/rates

Response example:
```
[{"id":1,"times":"0900-2100","days":"mon,tues,thurs","price":1500,"domainItem":{"days":{"sunday":false,"monday":true,"tuesday":true,"wednesday":false,"thursday":true,"friday":false,"saturday":false},"startTime":"09:00:00","endTime":"21:00:00","rateDuration":"12:00:00","price":1500,"rateItemId":1}},{"id":2,"times":"0900-2100","days":"fri,sat,sun","price":2000,"domainItem":{"days":{"sunday":true,"monday":false,"tuesday":false,"wednesday":false,"thursday":false,"friday":true,"saturday":true},"startTime":"09:00:00","endTime":"21:00:00","rateDuration":"12:00:00","price":2000,"rateItemId":2}}}]
```

Curl example:
```
curl -H "Content-Type: application/json" -G http://localhost:5000/api/rates
```

## Requesting a Price

Make a POST call to the `rateprice` endpoint specifying the start time and end time in a compatible datetime string format.


### api/rates/rateprice

Curl example:
```
curl -H "Content-Type: application/json" -d '{"starttime":"2015-07-01T07:00:00Z","endtime":"2015-07-01T08:00:00Z"}' http://localhost:5000/api/rates/rateprice
```

Response will be a number value of type `long` or the string literal "unavailable".

## Other calls

There are also `DELETE` and `UPDATE` calls as well as one for adding a single rate to the data. `UPDATE` is not up-to-date with the latest data schema, and `DELETE` does not currently delete the domain items. It is not recommended using either of these calls.

# iOS Client

A very thin iOS client which only calls the `rateprice` endpoint to request prices.

* Basic client-side validation
  * Client checks if end time is in the future of start time
  * If dates are incorrect, some simple UI tweaks indicate how to correct issues
* DateFormatter
  * Probably not necessary since the .NET datetime parser is robust enough to accept the default iOS date string format.

# Limitations

* These endpoints are JSON only. An XML format (if desired) should be easy to add, but I haven't.
* When dealing with time, timezones are always complicated and messy. In the case of this project, timezones are basically ignored. The project attempts to use local time or time offsets as much as possible.
* Days of the week are relative to the local timezone. In addition, only timespans within a single day are accounted for here. No multi-day or past-midnight rates are available.
* This project uses an in-memory db for rate storage. That won't scale well.
* There is no accounting for API keys, security groups, rate-limiting, etc. Basically, if you run this on the public internet as-is, you're likely to face `issues`.
