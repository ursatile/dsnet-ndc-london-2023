// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(connectToSignalR);

function connectToSignalR() {
	console.log("Connecting to SignalR...");
	const conn = new signalR.HubConnectionBuilder().withUrl("/hub").build();
	conn.on("ThisIsMagicStringNumberTwo",
		function (user, message) {
			console.log(user);
			console.log(message);
		});
	conn.start().then(function () {
		console.log("SignalR connected1");
	}).catch(function (err) {
		console.log(err);
	});
}
