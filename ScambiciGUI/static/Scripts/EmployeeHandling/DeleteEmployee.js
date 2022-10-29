$(document).ready(function() {

	let admin = {};
	admin['AdminId'] = $.session.get("employeeId");
	let timestamp = $.getTimestamp();
	admin['Signature'] = $.calculateSignature(timestamp);
	admin['Timestamp'] = timestamp;

	$.getJSON({
		url: EMPLOYEES_URL,
		data: $.param(admin),
		success: function (employeesArr) {
			employeesArr.forEach(function(employee, index) {
				if (employee.EmployeeId != admin['AdminId']){
					let employeeCard = $(`<div class="container list-container bg-body text-head rounded-lg shadow">
							<div class="text-head my-2">
								<span class="text-lg-head font-weight-bold">${employee.Name} ${employee.Surname}</span><br/>
							</div>
							<form id="employee-${index}" name="employee-${index}">
								<button type="submit" class="btn btn-block btn-head mt-3">Elimina dipendente</button>
							</form>
						</div>`);
					employeeCard.find("form").submit(function(event) {
						admin['EmployeeId'] = employee.EmployeeId;
						let timestamp = $.getTimestamp();
						admin['Signature'] = $.calculateSignature(timestamp);
						admin['Timestamp'] = timestamp;

						$.ajax({
							type: "POST",
							url: POST_DELETEEMPLOYEE_URL,
							data: JSON.stringify(admin),
							dataType: "json",
							contentType : "application/json",
							success: function() {
								alert("Eliminazione completata con successo!");
								location.reload();
							},
							error: function(data) {
								alert("Errore nell'invio della richiesta.");
							},
						});
					event.preventDefault();
					});
					$("#employees").append(employeeCard)
				} else {
					$("#store-address").html(employee['Store']['Address']);
				}
			});
		},
	});


});

