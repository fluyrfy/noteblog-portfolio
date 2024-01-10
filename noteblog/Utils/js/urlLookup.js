$("a").on("click", async function (event) {
	let targetUrl = $(this).attr("href");
	if (targetUrl && !isLocalUrl(targetUrl)) {
		event.preventDefault();
		targetUrl = encodeURIComponent(targetUrl);
		const trSB = await checkForSafeBrowsing(targetUrl);
		const trOPSWAT = await checkForOPSWAT(targetUrl);
		if (trSB || trOPSWAT) {
			const confirmed = confirm(
				"This link may be harmful. Do you want to continue?"
			);
			if (!confirmed) {
				return;
			}
		}
	}
	location.href = $(this).attr("href");
});

async function checkForSafeBrowsing(url) {
	try {
		const response = await fetch("/api/verify/url/safeBrowsing", {
			method: "POST",
			headers: {
				"Content-Type": "application/json",
			},
			body: JSON.stringify({
				client: {
					clientId: "fl-note",
					clientVersion: "1.0.0",
				},
				threatInfo: {
					threatTypes: [
						"MALWARE",
						"SOCIAL_ENGINEERING",
						"UNWANTED_SOFTWARE",
						"POTENTIALLY_HARMFUL_APPLICATION",
						"CSD_DOWNLOAD_WHITELIST",
					],
					platformTypes: ["ALL_PLATFORMS"],
					threatEntryTypes: ["URL"],
					threatEntries: [{ url }],
				},
			}),
		});

		const data = JSON.parse(await response.json());
		return (
			Object.getOwnPropertyNames(data).length > 0 && data.matches.length > 0
		);
	} catch (error) {
		console.error("Error checking URL:", error);
		return false;
	}
}

async function checkForOPSWAT(url) {
	try {
		const response = await fetch("/api/verify/url/OPSWAT", {
			method: "POST",
			headers: {
				"Content-Type": "application/json",
			},
			body: JSON.stringify({ url }),
		});

		const data = JSON.parse(await response.json());
		const lookupResults = data.lookup_results;
		if (lookupResults.detected_by > 0) {
			const sources = lookupResults.sources;
			for (const source of sources) {
				if (source.status === 1) {
					return true;
				}
			}
		}
		return false;
	} catch (error) {
		console.error("Error checking OPSWAT:", error);
		throw error;
	}
}

function isLocalUrl(url) {
	if (!url.startsWith("http://") && !url.startsWith("https://")) {
		return true;
	}

	const currentHostname = window.location.hostname;
	const targetHostname = new URL(url).hostname;
	return currentHostname === targetHostname;
}
