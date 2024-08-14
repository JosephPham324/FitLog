import React, { Component } from 'react';
import axiosInstance from '../utils/axiosInstance';
import { Container, Grid, TextField, Button, List, ListItem, ListItemText, Dialog, DialogTitle, DialogContent, DialogActions, Typography, MenuItem, Pagination, Alert, Paper } from '@mui/material'; // Ensure MenuItem is imported

export class CoachServiceBooking extends Component {
  constructor(props) {
    super(props);
    this.state = {
      services: [],
      searchQuery: '',
      clientName: '',
      clientEmail: '',
      clientPhone: '',
      bookingDate: '',
      selectedServiceId: null,
      successMessage: '',
      selectedServiceDetails: null,
      showModal: false,
      filters: {
        durationMin: '',
        durationMax: '',
        priceMin: '',
        priceMax: '',
        availability: ''
      },
      currentPage: 1,
      totalPages: 1
    };
    this.searchServices = this.searchServices.bind(this);
    this.handleInputChange = this.handleInputChange.bind(this);
    this.handleDetailClick = this.handleDetailClick.bind(this);
    this.handleCloseModal = this.handleCloseModal.bind(this);
    this.handleFilterChange = this.handleFilterChange.bind(this);
    this.fetchServices = this.fetchServices.bind(this);
    this.handleBooking = this.handleBooking.bind(this);
  }

  componentDidMount() {
    this.fetchServices();
  }

  async fetchServices(pageNumber = 1, pageSize = 5) {
    try {
      const response = await axiosInstance.get(`/CoachingServices?PageNumber=${pageNumber}&PageSize=${pageSize}`);
      this.setState({
        services: response.data.items,
        currentPage: pageNumber,
        totalPages: Math.ceil(response.data.totalItems / pageSize)
      });
    } catch (error) {
      console.error('Error fetching services:', error);
    }
  }

  searchServices(event) {
    const query = event.target.value.toLowerCase();
    this.setState({ searchQuery: query });
  }

  handleFilterChange(event) {

    const { name, value } = event.target;
    console.log(event.target)
    // Ensure only positive numbers are entered
    if (value === '' || parseInt(value) >= 0) {
      this.setState(prevState => ({
        filters: {
          ...prevState.filters,
          [name]: value
        }
      }));
    }
    if (name === 'availability') {
      this.setState(prevState => ({
        filters: {
          ...prevState.filters,
          [name]: value
        }
      }));
    }
  }

  handleInputChange(event) {
    const { name, value } = event.target;
    this.setState({
      [name]: value,
    });
  }

  async handleDetailClick(serviceId) {
    try {
      const response = await axiosInstance.get(`/CoachingServices/${serviceId}`);
      this.setState({
        selectedServiceId: serviceId,
        selectedServiceDetails: response.data,
        successMessage: '',
        showModal: true,
      });
    } catch (error) {
      console.error('Detail fetch error:', error.response?.data || error.message);
      this.setState({
        selectedServiceDetails: null,
        showModal: true,
      });
    }
  }

  handleCloseModal() {
    this.setState({ showModal: false });
  }

  async handleBooking() {
    try {
      const { selectedServiceId } = this.state;
      await axiosInstance.post(`/CoachingServices/book/${selectedServiceId}`, {
        coachingServiceId: selectedServiceId
      });
      this.setState({
        successMessage: 'Booking successful!',
        showModal: false,
      });
    } catch (error) {
      console.error('Booking error:', error.response?.data || error.message);
      this.setState({
        successMessage: 'Booking failed!',
      });
    }
  }

  render() {
    const { services, searchQuery, selectedServiceDetails, showModal, filters, currentPage, totalPages, successMessage } = this.state;
    const filteredServices = services.filter(service => {
      const query = searchQuery.toLowerCase();
      const { serviceName, duration, price, serviceAvailability } = service;
      const availabilityMatch = filters.availability === '' ||
        (filters.availability === 'Available' && serviceAvailability === true) ||
        (filters.availability === 'Not Available' && serviceAvailability === false);
      return (
        serviceName.toLowerCase().includes(query) &&
        (filters.durationMin === '' || filters.durationMax === '' || (duration >= filters.durationMin && duration <= filters.durationMax)) &&
        (filters.priceMin === '' || filters.priceMax === '' || (price >= filters.priceMin && price <= filters.priceMax)) &&
        availabilityMatch
      );
    });

    return (
      <Container>
        <Typography variant="h3" align="center" gutterBottom style={{ marginTop: '20px', color: '#3f51b5' }}>
          Coach Service Booking
        </Typography>
        <Grid container spacing={3} alignItems="flex-end" style={{ marginBottom: '20px' }}>
          <Grid item xs={12} sm={8}>
            <TextField
              fullWidth
              label="Search Services"
              variant="outlined"
              value={searchQuery}
              onChange={this.searchServices}
              size="large"
            />
          </Grid>
        </Grid>
        <Grid container spacing={3} style={{ marginBottom: '20px' }}>
          <Grid item xs={12} sm={4}>
            <Grid container spacing={2}>
              <Grid item xs={6}>
                <TextField
                  fullWidth
                  label="Duration Min (weeks)"
                  name="durationMin"
                  value={filters.durationMin}
                  onChange={this.handleFilterChange}
                  variant="outlined"
                  type="number"
                  inputProps={{ min: 0 }} // Ensure only positive numbers
                />
              </Grid>
              <Grid item xs={6}>
                <TextField
                  fullWidth
                  label="Duration Max (weeks)"
                  name="durationMax"
                  value={filters.durationMax}
                  onChange={this.handleFilterChange}
                  variant="outlined"
                  type="number"
                  inputProps={{ min: 0 }} // Ensure only positive numbers
                />
              </Grid>
            </Grid>
          </Grid>
          <Grid item xs={12} sm={4}>
            <Grid container spacing={2}>
              <Grid item xs={6}>
                <TextField
                  fullWidth
                  label="Price Min"
                  name="priceMin"
                  value={filters.priceMin}
                  onChange={this.handleFilterChange}
                  variant="outlined"
                  type="number"
                  inputProps={{ min: 0 }} // Ensure only positive numbers
                />
              </Grid>
              <Grid item xs={6}>
                <TextField
                  fullWidth
                  label="Price Max"
                  name="priceMax"
                  value={filters.priceMax}
                  onChange={this.handleFilterChange}
                  variant="outlined"
                  type="number"
                  inputProps={{ min: 0 }} // Ensure only positive numbers
                />
              </Grid>
            </Grid>
          </Grid>
          <Grid item xs={12} sm={4}>
            <TextField
              select
              fullWidth
              label="Availability"
              name="availability"
              value={filters.availability}
              onChange={this.handleFilterChange}
              variant="outlined"
            >
              <MenuItem value="">All</MenuItem>
              <MenuItem value="Available">Available</MenuItem>
              <MenuItem value="Not Available">Not Available</MenuItem>
            </TextField>
          </Grid>
        </Grid>
        <Paper elevation={3}>
          <List>
            {filteredServices.length > 0 ? (
              filteredServices.map(service => (
                <ListItem key={service.id} divider style={{ padding: '20px' }}>
                  <ListItemText
                    primary={<Typography variant="h6" style={{ color: '#3f51b5' }}>{service.serviceName}</Typography>}
                    secondary={`Duration: ${service.duration} Weeks, Price: $${service.price}`}
                  />
                  <Button variant="contained" color="primary" onClick={() => this.handleDetailClick(service.id)}>
                    Booking
                  </Button>
                </ListItem>
              ))
            ) : (
              <Typography variant="h6" align="center" style={{ margin: '20px', color: 'red' }}>
                No services found.
              </Typography>
            )}
          </List>
        </Paper>
        <Grid container justifyContent="center" style={{ marginTop: '20px' }}>
          <Pagination
            count={totalPages}
            page={currentPage}
            onChange={(event, value) => this.fetchServices(value)}
            color="primary"
            size="large"
          />
        </Grid>
        <Dialog open={showModal} onClose={this.handleCloseModal} fullWidth maxWidth="sm">
          <DialogTitle>Service Detail</DialogTitle>
          <DialogContent>
            {selectedServiceDetails ? (
              <>
                <Typography variant="h6"><strong>Service Name:</strong> {selectedServiceDetails.serviceName}</Typography>
                <Typography><strong>Description:</strong> {selectedServiceDetails.description}</Typography>
                <Typography><strong>Duration:</strong> {selectedServiceDetails.duration} Weeks</Typography>
                <Typography><strong>Price:</strong> ${selectedServiceDetails.price}</Typography>
                <Typography><strong>Availability:</strong> {selectedServiceDetails.serviceAvailability ? 'Available' : 'Not Available'}</Typography>
                <Typography><strong>Announcement:</strong> {selectedServiceDetails.availabilityAnnouncement}</Typography>
                <Typography><strong>Created By:</strong> {selectedServiceDetails.createdByUserName}</Typography>
                <Typography><strong>Last Modified By:</strong> {selectedServiceDetails.lastModifiedByUserName}</Typography>
                {successMessage && <Alert severity="success">{successMessage}</Alert>}
              </>
            ) : (
              <Typography>Service not found.</Typography>
            )}
          </DialogContent>
          <DialogActions>
            <Button onClick={this.handleCloseModal} color="secondary" variant="contained">
              Close
            </Button>
            {selectedServiceDetails && (
              <Button onClick={this.handleBooking} color="primary" variant="contained">
                Book
              </Button>
            )}
          </DialogActions>
        </Dialog>
      </Container>
    );
  }
}

export default CoachServiceBooking;
