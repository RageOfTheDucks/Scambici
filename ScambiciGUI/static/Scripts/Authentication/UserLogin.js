$(document).ready(function() {

	$('#user-login-form').submit(function(event) {
		let formUser = $('#user-login-form').serializeObject();
		let jsSha = new jsSHA("SHA-512", "TEXT");
		jsSha.update(formUser['PasswordHash']);
		let hash = jsSha.getHash("HEX");
		formUser['PasswordHash'] = hash;
		$.session.set("passwordHash", hash)
		let timestamp = $.getTimestamp();
		formUser['Signature'] = $.calculateSignature(timestamp);
		formUser['Timestamp'] = timestamp;

		$.ajax({ 
			type: "POST",
			url: POST_USER_LOGIN_URL,
			data: JSON.stringify(formUser),
			dataType: "json",
			contentType : "application/json",
			success: function(data, textStatus, jqXHR) {
				$.session.set("userId", data['UserId']);
				$.session.set("name", data['Name']);
				$.session.set("surname", data['Surname']);
				if (data['RentedBike'] != null) {
					window.location.href = "/Scambici/ActiveUser/HomeActiveUser";
				} else {
					window.location.href = "/Scambici/InactiveUser/HomeInactiveUser";
				}
			},
			error: function(data, textStatus, jqXHR) {
				alert("Indirizzo e-mail o password errati!");
			}
		});
		event.preventDefault();

	});
});
