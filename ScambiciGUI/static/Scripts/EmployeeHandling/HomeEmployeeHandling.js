$(document).ready(function(){
	let name = $.session.get("name");
	let surname = $.session.get("surname");
	let isAdmin = false;
	$.getJSON(GET_STORES_URL, function(stores) {
		for (i in  stores) {
			if (stores[i].AdministratorId == $.session.get("employeeId")) {

				$("#store-address").html(stores[i].Address);
				isAdmin = true;
			}
		}
		if (!isAdmin) {
			alert("Questa interfaccia è riservata agli amministratori. Il tuo tentativo di accesso è stato registrato.");
			window.location.href = "/Scambici/Authentication/AdminLogout";
		}
	});



	$("#fullname").html(name +" "+ surname);
});

