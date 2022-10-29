$(document).ready(function() {
	$.session.remove("employeeId");
	$.session.remove("name");
	$.session.remove("surname");
	$.session.remove("passwordHash");
	window.location.href = "/Scambici/Authentication/HomeAccess/";
});
