// tslint:disable-next-line
const AutoNumeric = require('autonumeric');
import { INumberFormats } from '@round/number';

// More info here
// http://autonumeric.org/
// http://autonumeric.org/guide
// http://autonumeric.org/examples
// https://github.com/autoNumeric/autoNumeric/blob/next/src/AutoNumericOptions.js
// https://github.com/autoNumeric/autoNumeric/blob/next/src/AutoNumericPredefinedOptions.js
const predefinedOptions = AutoNumeric.predefinedOptions;
export const autonumericFormats: INumberFormats = { // this object will be passed to the numberService.init
	'en-US': {
		percentage: {
			...predefinedOptions.percentageUS2dec,
			...{
			maximumValue: 100,
			minimumValue: 0
			}
		},
		integer: {
			decimalPlaces: 0
		},
		decimal: 'dotDecimalCharCommaSeparator',
		euro: {
			...predefinedOptions.euro,
			...{
			digitGroupSeparator: ',',
			decimalCharacter: '.'
			}
		},
		dollar: {
			...predefinedOptions.dollar,
			...{
				currencySymbolPlacement: 's',
				negativePositiveSignPlacement: 'p'
			}
		}
	},
	'it-IT': {
		percentage: {
			...predefinedOptions.percentageEU2dec,
			...{
				maximumValue: 100,
				minimumValue: 0
			}
		},
		integer: {
			decimalPlaces: 0,
			digitGroupSeparator: '.',
			decimalCharacter: ','
		},
		decimal: 'commaDecimalCharDotSeparator',
		euro: {
			...predefinedOptions.euro
		},
		dollar: {
			...predefinedOptions.dollar,
			...{
				currencySymbolPlacement: 's',
				negativePositiveSignPlacement: 'p',
				digitGroupSeparator: '.',
				decimalCharacter: ','
			}
		}
	}
};
