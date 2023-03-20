import moment, { LocaleSpecification } from 'moment';

const en = moment.localeData('en') as any;
const it = moment.localeData('it') as any;

// the key is in format of language-culture if the langauge is different from the culture otherwise is just culture
// i.e. culture is it-IT and language is en -> en-it-IT
//      culture is it-IT and language is it -> it-IT
// This is done because users may have culture and language that are not aligned
export const dateFormats: {
	[key: string]: LocaleSpecification
} = {
	'en-it-IT': {
		months: [ ...en._months ], // language en
		monthsShort: [ ...en._monthsShort ], // language en
		weekdays: [...en._weekdays], // language en
		weekdaysShort: [...en._weekdaysShort], // language en
		weekdaysMin: [...en._weekdaysMin], // language en

		longDateFormat: { ...it._longDateFormat }, // culture it-IT

		calendar: { ...en._calendar }, // language en
		invalidDate: en._invalidDate, // language en
		relativeTime: { ...en._relativeTime }, // language en
		dayOfMonthOrdinalParse: new RegExp(en._dayOfMonthOrdinalParse), // language en
		ordinal: en.ordinal, // language en
		week: it._week,  // culture it-IT

		isPM: it.isPM, // culture it-IT
		meridiem: it.meridiem,  // culture it-IT
		meridiemParse: new RegExp(it._meridiemParse)  // culture it-IT
	},
	'it-en-US': {
		months: [ ...it._months ],
		monthsShort: [ ...it._monthsShort ],
		weekdays: [...it._weekdays],
		weekdaysShort: [...it._weekdaysShort],
		weekdaysMin: [...it._weekdaysMin],

		longDateFormat: { ...en._longDateFormat },

		calendar: { ...it._calendar },
		invalidDate: it._invalidDate,
		relativeTime: { ...it._relativeTime },
		dayOfMonthOrdinalParse: new RegExp(it._dayOfMonthOrdinalParse),
		ordinal: it.ordinal,
		week: en._week,

		isPM: en.isPM,
		meridiem: en.meridiem,
		meridiemParse: new RegExp(en._meridiemParse)
	}
};
