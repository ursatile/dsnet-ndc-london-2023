// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(connectToSignalR);

function connectToSignalR() {
	console.log("Connecting to SignalR...");
	const conn = new signalR.HubConnectionBuilder().withUrl("/hub").build();
	conn.on("DisplayPriceNotification", DisplayPriceNotification);
	conn.start().then(function () {
		console.log("SignalR connected1");
	}).catch(function (err) {
		console.log(err);
	});
}

function DisplayPriceNotification(user, message) {
	console.log(user);
	console.log(message);
	const data = JSON.parse(message);

	var html = `<div>New vehicle! ${data.Make} ${data.Model}<br />
${data.Color}, ${data.Year}. Price ${data.Price} ${data.CurrencyCode}<br />
<a href="/vehicles/details/${data.Registration}">click here for more...<a/></div>`;
	var $html = $(html);
	$html.css("background-color", data.Color);
	var $target = $("#signalr-notifications");
	$target.prepend($html);
	window.setTimeout(function() {
		 $html.fadeOut(2000, function() {
			  $html.remove();
		 });
	}, 2000);


}
/*
 * AutobarnCorrelationId
:
"a3313758-651c-46bc-b8ea-573982f11d34"
Color
:
"Gold"
CurrencyCode
:
"EUR"
ListedAt
:
"2023-01-24T15:43:39.439706+00:00"
Make
:
"FIAT"
Model
:
"500"
Price
:
19850
Registration
:
"JNKRAT"
Year
:
1985
*/
