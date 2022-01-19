import * as React from 'react';
import TextField from '@mui/material/TextField';
import AdapterDateFns from '@mui/lab/AdapterDateFns';
import LocalizationProvider from '@mui/lab/LocalizationProvider';
import TimePicker from '@mui/lab/TimePicker';
import {useInput} from "react-admin";


const TimeInput = (props) =>{
    const { input, meta: { touched, error }} = useInput(props);
    console.log(input.value);
    const handleChange = value => {
        Date.parse(value) ? input.onChange(value.toISOString()) : input.onChange(null);
    };

    return (
        <LocalizationProvider dateAdapter={AdapterDateFns}>
            <TimePicker
                ampm={false}
                //name = {name}
                openTo="hours"
                views={['hours', 'minutes']}
                inputFormat="HH:mm"
                mask="__:__"
                label={props.label}
                error={!!(touched && error)}
                helperText={touched && error}
                value={input.value}
                onChange={date => handleChange(date)}
                renderInput={(params) => <TextField  {...params}
                />}
            />
        </LocalizationProvider>
    );
}

export default TimeInput;