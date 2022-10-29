$(document).ready(function(){
	$("#delete-user").submit(function(event) {
		let deleteUser = $('#delete-user').serializeObject();
		let passwordHash = deleteUser['password']
		let jsSha = new jsSHA("SHA-512", "TEXT");
		jsSha.update(passwordHash);
		let hash = jsSha.getHash("HEX");
		if (hash != $.session.get("passwordHash")) {
			alert("Password errata");
			return false;
		}

		deleteUser['UserId'] = $.session.get("userId");
		let timestamp = $.getTimestamp();
		deleteUser['Signature'] = $.calculateSignature(timestamp);
		deleteUser['Timestamp'] = timestamp;

		$.ajax({ 
			type: "POST",
			url: POST_DELETEUSER_URL,
			data: JSON.stringify(deleteUser),
			dataType: "json",
			contentType : "application/json",
			success: function() {
				window.location.href = "/Scambici/Authentication/HomeAccess";
			}, 
			error: function() {
				alert("Errore nell'invio della richiesta");
			},
		});
		event.preventDefault();
	});
});
