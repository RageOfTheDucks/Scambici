const GET_STORES_URL = "https://scambici.azurewebsites.net/api/Stores";
const POST_REGISTERUSER_URL = "https://scambici.azurewebsites.net/api/UserRegistration";
const POST_MAINTENANCEREQ_URL = "https://scambici.azurewebsites.net/api/MaintenanceRequest";
const POST_THEFTREPORT_URL = "https://scambici.azurewebsites.net/api/TheftReport";
const POST_RENTALINTERRUPT_URL = "https://scambici.azurewebsites.net/api/RentalInterruption";
const POST_DELETEUSER_URL = "https://scambici.azurewebsites.net/api/UserDeletion";
const POST_NEWBIKE_URL = "https://scambici.azurewebsites.net/api/NewBikeRequest";
const POST_USER_LOGIN_URL = "https://scambici.azurewebsites.net/api/UserLogin";
const POST_EMPLOYEE_LOGIN_URL = "https://scambici.azurewebsites.net/api/EmployeeLogin";
const POST_BIKESCRAP_URL = "https://scambici.azurewebsites.net/api/BikeScrap";
const POST_BIKESWAP_URL = "https://scambici.azurewebsites.net/api/BikeSwap";
const POST_THEFTCONFIRMATION_URL = "https://scambici.azurewebsites.net/api/TheftConfirmation";
const POST_GETUSERMAINTREQ_URL = "https://scambici.azurewebsites.net/api/GetUserMaintenances";
const POST_GETSTOREMAINTREQ_URL = "https://scambici.azurewebsites.net/api/GetStoreMaintenances";
const POST_GETRENTALINTERRUPTREQ_URL = "https://scambici.azurewebsites.net/api/GetRentalInterruptions";
const POST_GETDELIVERIESREQ_URL = "https://scambici.azurewebsites.net/api/GetBikeDeliveries";
const POST_GETTHEFTREPORTREQ_URL = "https://scambici.azurewebsites.net/api/GetTheftReports";
const POST_ASSIGN_USERMAINT_URL = "https://scambici.azurewebsites.net/api/AssignUserMaintenance";
const POST_ASSIGN_STOREMAINT_URL = "https://scambici.azurewebsites.net/api/AssignStoreMaintenance";
const POST_ASSIGN_DELIVERY_URL = "https://scambici.azurewebsites.net/api/AssignBikeDelivery";
const POST_TERMINATE_USERMAINT_URL = "https://scambici.azurewebsites.net/api/TerminateUserMaintenance";
const POST_TERMINATE_STOREMAINT_URL = "https://scambici.azurewebsites.net/api/TerminateStoreMaintenance";
const POST_TERMINATE_DELIVERY_URL = "https://scambici.azurewebsites.net/api/TerminateBikeDelivery";
const EMPLOYEES_URL = "https://scambici.azurewebsites.net/api/Employees";
const POST_DELETEEMPLOYEE_URL = "https://scambici.azurewebsites.net/api/EmployeeDeletion";
const POST_REGISTEREMPLOYEE_URL = "https://scambici.azurewebsites.net/api/EmployeeRegistration";
const POST_RENTALINTERRUPTCONF_URL = "https://scambici.azurewebsites.net/api/RentalInterruptionConfirmation";
const POST_THEFTREPORTCONF_URL = "https://scambici.azurewebsites.net/api/TheftReportConfirmation";
const POST_GETCURRENTTASK_URL = "https://scambici.azurewebsites.net/api/GetCurrentTask";

$.fn.serializeObject = function() {
	let o = {};
	let a = this.serializeArray();
	$.each(a, function() {
		if (o[this.name]) {
			if (!o[this.name].push) {
				o[this.name] = [o[this.name]];
			}
			o[this.name].push(this.value || '');
		} else {
			o[this.name] = this.value || '';
		}
	});
	return o;
};

$.getTimestamp = function() {
	timestamp = Math.floor(Date.now() / 1000);
	return ""+timestamp;
};

$.calculateSignature = function(timestamp) {
	// call $.session.set("passwordHash", VALUE_OF_PASS_HASH) on login
	hash = $.session.get("passwordHash");
	const shaObj = new jsSHA("SHA-512", "TEXT", {
		hmacKey: { value: hash, format: "TEXT" },
	});
	shaObj.update(timestamp);
	return shaObj.getHash("HEX");
};

function includeHTML() {
	var z, i, elmnt, file, xhttp;
	/*loop through a collection of all HTML elements:*/
	z = document.getElementsByTagName("*");
	for (i = 0; i < z.length; i++) {
		elmnt = z[i];
		/*search for elements with a certain atrribute:*/
		file = elmnt.getAttribute("include-html");
		if (file) {
			/*make an HTTP request using the attribute value as the file name:*/
			xhttp = new XMLHttpRequest();
			xhttp.onreadystatechange = function() {
				if (this.readyState == 4) {
					if (this.status == 200) {elmnt.innerHTML = this.responseText;}
					if (this.status == 404) {elmnt.innerHTML = "Page not found.";}
					/*remove the attribute, and call this function once more:*/
					elmnt.removeAttribute("include-html");
					includeHTML();
				}
			}
			xhttp.open("GET", file, true);
			xhttp.send();
			/*exit the function:*/
			return;
		}
	}
};

//check session
$(document).ready(function(){
	var ar = [
		"",
		"/ActiveUser",
		"/InactiveUser",
		"/ActiveUser/Confirmation",
		"/Employee",
		"/Employee/HomeEmployeeTask",
		"/EmployeeHandling"
	];
	var pathname = window.location.pathname;
	var hostname = pathname.substring(0, pathname.indexOf("Templates") + 9);
	var path = pathname.substring(hostname.length, pathname.lastIndexOf("/"));

	if (ar.includes(path)) {
		if (($.session.get("userId") === undefined && $.session.get("employeeId") === undefined) || $.session.get("name") === undefined ||
			$.session.get("surname") === undefined || $.session.get("passwordHash") === undefined) {
			window.location.href = "/Scambici/Authentication/HomeAccess";
		}
	}
});
