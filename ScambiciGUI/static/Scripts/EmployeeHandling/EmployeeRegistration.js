$(document).ready(function() {
	$("#registration-admin").submit(function(event) {
		let formEmployee = $('#registration-admin').serializeObject();
		$.session.set("registrandName", formEmployee["Name"]);
		$.session.set("registrandSurname", formEmployee["Surname"]);
		window.location.href = "/Scambici/EmployeeHandling/EmployeeRegistration_Employee";
		event.preventDefault();
	});
});
