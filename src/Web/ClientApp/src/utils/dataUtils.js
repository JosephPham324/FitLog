import moment from 'moment';

/**
 * Generates continuous data for a given time frame.
 * @param {Object} data - The original data object with date keys in ISO format.
 * @param {String} timeFrame - The time frame ('weekly', 'monthly', 'yearly').
 * @returns {Object} - A new object with continuous data.
 */
const generateContinuousData = (data, timeFrame) => {
  const formatMap = {
    weekly: 'YYYY-MM-DD',
    monthly: 'YYYY-MM',
    yearly: 'YYYY',
  };

  const startDate = moment(Object.keys(data)[0]);
  const endDate = moment(Object.keys(data)[Object.keys(data).length - 1]);
  const continuousData = {};

  let currentDate = startDate.clone();

  while (currentDate.isSameOrBefore(endDate)) {
    const originalFormat = currentDate.toISOString(); // Get the date in original format
    const formattedDate = currentDate.format(formatMap[timeFrame]);

    continuousData[formattedDate] = data[originalFormat] || 0; // Use the original date format to look up data

    // Increment the date based on the time frame
    if (timeFrame === 'weekly') {
      currentDate.add(1, 'week');
    } else if (timeFrame === 'monthly') {
      currentDate.add(1, 'month');
    } else if (timeFrame === 'yearly') {
      currentDate.add(1, 'year');
    }
  }

  return continuousData;
};

export default generateContinuousData;
