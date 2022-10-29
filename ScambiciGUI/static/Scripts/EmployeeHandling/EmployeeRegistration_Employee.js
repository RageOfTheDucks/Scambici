$(document).ready(function() {
	let registrandName = $.session.get("registrandName");
	let registrandSurname = $.session.get("registrandSurname");
	$("#registrand-name").html(registrandName + " " + registrandSurname);
	$("#registration-employee").submit(function(event) {
		formRegistrand = $("#registration-employee").serializeObject();
		const jsSha = new jsSHA("SHA-512", "TEXT", { numRounds: 1});
		jsSha.update(formRegistrand["RegistrandPasswordHash"]);
		let hash = jsSha.getHash("HEX", {outputUpper: false});
		formRegistrand["RegistrandPasswordHash"] = hash;
		formRegistrand["RegistrandName"] = registrandName;
		formRegistrand["RegistrandSurname"] = registrandSurname; 
		let timestamp = $.getTimestamp();
		formRegistrand["Signature"] = $.calculateSignature(timestamp);
		formRegistrand["Timestamp"] = timestamp;
		formRegistrand["EmployeeId"] =  $.session.get("employeeId");
		console.log(formRegistrand);
		$.ajax({
			type: "POST",
			url: POST_REGISTEREMPLOYEE_URL,
			data: JSON.stringify(formRegistrand),
			dataType: "json",
			contentType : "application/json",
			success: function() {
				alert("Registrazione completata con successo!");
				window.location.href = "/Scambici/EmployeeHandling/HomeEmployeeHandling";
			},
			error: function(data) {
				alert("Errore nell'invio della richiesta.");
			},
		});


		event.preventDefault();
	});
});
