import React, { Component } from 'react';
import axiosInstance from '../utils/axiosInstance';
import { Container, Grid, TextField, Button, List, ListItem, ListItemText, Dialog, DialogTitle, DialogContent, DialogActions, Typography, MenuItem, Pagination, Alert, Paper } from '@mui/material';

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
        duration: '',
        price: '',
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
    this.setState(prevState => ({
      filters: {
        ...prevState.filters,
        [name]: value
      }
    }));
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
      return (
        serviceName.toLowerCase().includes(query) &&
        (filters.duration === '' ||
          (filters.duration === '1-3' && duration >= 1 && duration <= 3) ||
          (filters.duration === '4-6' && duration >= 4 && duration <= 6) ||
          (filters.duration === '7-10' && duration >= 7 && duration <= 10) ||
          (filters.duration === '>10' && duration > 10)) &&
        (filters.price === '' ||
          (filters.price === '<100' && price < 100) ||
          (filters.price === '100-500' && price >= 100 && price <= 500) ||
          (filters.price === '501-1000' && price >= 501 && price <= 1000) ||
          (filters.price === '>1000' && price > 1000)) &&
        (filters.availability === '' || (filters.availability === 'Available' ? serviceAvailability : !serviceAvailability))
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
            <TextField
              select
              fullWidth
              label="Duration (weeks)"
              name="duration"
              value={filters.duration}
              onChange={this.handleFilterChange}
              variant="outlined"
            >
              <MenuItem value="">All</MenuItem>
              <MenuItem value="1-3">1-3 weeks</MenuItem>
              <MenuItem value="4-6">4-6 weeks</MenuItem>
              <MenuItem value="7-10">7-10 weeks</MenuItem>
              <MenuItem value=">10">More than 10 weeks</MenuItem>
            </TextField>
          </Grid>
          <Grid item xs={12} sm={4}>
            <TextField
              select
              fullWidth
              label="Price"
              name="price"
              value={filters.price}
              onChange={this.handleFilterChange}
              variant="outlined"
            >
              <MenuItem value="">All</MenuItem>
              <MenuItem value="<100">Less than $100</MenuItem>
              <MenuItem value="100-500">$100 - $500</MenuItem>
              <MenuItem value="501-1000">$501 - $1000</MenuItem>
              <MenuItem value=">1000">More than $1000</MenuItem>
            </TextField>
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
            {filteredServices.map(service => (
              <ListItem key={service.id} divider style={{ padding: '20px' }}>
                <ListItemText
                  primary={<Typography variant="h6" style={{ color: '#3f51b5' }}>{service.serviceName}</Typography>}
                  secondary={`Duration: ${service.duration} Weeks, Price: $${service.price}`}
                />
                <Button variant="contained" color="primary" onClick={() => this.handleDetailClick(service.id)}>
                  Booking
                </Button>
              </ListItem>
            ))}
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
              <Typography>Loading...</Typography>
            )}
          </DialogContent>
          <DialogActions>
            <Button onClick={this.handleCloseModal} color="secondary" variant="contained">
              Close
            </Button>
            <Button onClick={this.handleBooking} color="primary" variant="contained">
              Book
            </Button>
          </DialogActions>
        </Dialog>
      </Container>
    );
  }
}

export default CoachServiceBooking;
