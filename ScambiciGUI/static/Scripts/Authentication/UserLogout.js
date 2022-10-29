$(document).ready(function() {
		$.session.remove("userId");
		$.session.remove("name");
		$.session.remove("surname");
		$.session.remove("passwordHash");
		window.location.href = "/Scambici/Authentication/HomeAccess";
});
