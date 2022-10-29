$(document).ready(function() {

	$('#employee-login-form').submit(function(event) {
		let formEmployee = $('#employee-login-form').serializeObject();
		let jsSha = new jsSHA("SHA-512", "TEXT");
		jsSha.update(formEmployee['PasswordHash']);
		let hash = jsSha.getHash("HEX");
		formEmployee['PasswordHash'] = hash;
		$.session.set("passwordHash", hash);
		let timestamp = $.getTimestamp();
		formEmployee['Signature'] = $.calculateSignature(timestamp);
		formEmployee['Timestamp'] = timestamp;

		$.ajax({ 
			type: "POST",
			url: POST_EMPLOYEE_LOGIN_URL,
			data: JSON.stringify(formEmployee),
			dataType: "json",
			contentType : "application/json",
			success: function(data, textStatus, jqXHR) {
				console.log("success");
				$.session.set("employeeId", data['EmployeeId']);
				$.session.set("name", data['Name']);
				$.session.set("surname", data['Surname']);
				window.location.href = "/Scambici/EmployeeHandling/HomeEmployeeHandling";
			},
			error: function(data, textStatus, jqXHR) {
				alert("Indirizzo e-mail o password errati!");
			}
		});
		event.preventDefault();

	});
});
