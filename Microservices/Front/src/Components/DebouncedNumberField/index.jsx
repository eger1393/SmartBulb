import React, {useState} from 'react'
import {TextField} from '@material-ui/core'
import {useDebounce} from "react-use";

const DebouncedNumberField = (props) => {
    const [value, setValue] = useState(props.value);
    const [, cancel] = useDebounce(
        () => {
            if (props.debounceCallback && props.debounce) props.debounceCallback();
        },
        props.debounce ?? 0,
        [value]);
    const handleChange = (e) => {
        const {max, min} = props;
        const val = e.target.value;
        if((val !== undefined && max !== undefined  && min !== undefined) && (val < min || val > max)) {
            console.log('validate error');
            return;
        }
        setValue(val);
        props.onChange && props.onChange(e);
    };
    return (<TextField
        {...props}
        onChange={handleChange}
    />)
};

export default DebouncedNumberField